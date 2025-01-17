using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay.GameData;
using Manager;
using Player.AutoAttacks;
using Player.Spells_Effects;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
using Utils;

using ClockGame = Gameplay.GameData.ClockGame;

namespace Player
{
    public class Character : SingletonMonoBehaviour<Character>
    {
        public static readonly int Hurt = Animator.StringToHash("hurt");
        public static readonly int DeathState = Animator.StringToHash("isDead");
        public static readonly int Dissolve = Shader.PropertyToID("_State");

        public static Action OnPlayerDeath;
        public static Action<int> OnHpChanged;
        public static Action<int> OnMaxHpChanged;
        public static Action<int> OnExpChanged;
        public static Action<int> OnSpellIndexChange;
        public static Action OnLevelUp;
        public static Action<Spell, Spell> OnSpellUnlock;
        public static Action OnDisplayUpgrade;
        public static Action<int> OnUpgradeStat;
        public static Action<bool> OnSpeedBoost;

        [Header("References")]
        public Transform EnemyRaycastTarget;
        [SerializeField] private Animator _playerAnimator;
        [SerializeField] private List<Renderer> _renderers;
        [SerializeField] private GameObject _speedFX;

        [Header("Spell Data List")]
        [SerializeField] private Spell[] _spells;

        [Header("Base Stats")]
        [SerializeField] private int _baseMaxHP;
        [SerializeField] private float _baseSpeed = 500f;
        [SerializeField] private int _baseEXP;
        [SerializeField] private float _baseSpellDamage;
        [SerializeField] private float _attackCooldown = 3;

        [Header("Progression")]
        [SerializeField] private float _cooldownUpgrade = -0.25f;
        [SerializeField] private int _hpPerConst;

        [Header("Timers")]
        [SerializeField] private float _boostDelay = 3;
        [SerializeField] private float _deathAnimationDuration = 2;

        [Header("VFX")]
        [SerializeField] private VisualEffect _vfxGraph;
        [SerializeField] private List<ParticleSystem> _particleSystem;
        
        public int Constitution { get; set; }
        public int Swiftness { get; set; }
        public int Power { get; set; }
        public int SpellUnlock{ get; set; }
        
        public int Level
        {
            get => _level;
            set
            {
                _level = value;
                OnLevelUp?.Invoke();
            }
        }
        public int RequireEXP => Mathf.CeilToInt(_baseEXP * Mathf.Pow(Level, 1.5f));
        public int EXP
        {
            get => _exp;
            set
            {
                _exp = Mathf.Clamp(value, 0, RequireEXP);

                if (_exp >= RequireEXP) {
                    _exp -= RequireEXP;
                    LevelUp();
                }

                OnExpChanged?.Invoke(_exp);
            }
        }
        public int MaxHP
        {
            get => _maxHp;
            set
            {
                _maxHp = value;
                OnMaxHpChanged?.Invoke(_maxHp);
            }
        }
        public int HP
        {
            get => _hp;
            set
            {
                _hp = Mathf.Clamp(value, 0, MaxHP);
                OnHpChanged?.Invoke(_hp);
            }
        }
        public float Speed
        {
            get => _baseSpeed;
            private set => _baseSpeed = value;
        }
        public float AttackCooldown
        {
            get => _attackCooldown;
            set => _attackCooldown = value;
        }
        public float SpellPower
        {
            get => _baseSpellDamage;
            private set => _baseSpellDamage = value;
        }

        public Spell CurrentSpell { get; private set; }
        public Spell NextSpell { get; private set; }
        public bool IsBoosted { get; private set; }
        public bool IsDead { get; private set; }

        private int _level = 1;
        private int _hp;
        private int _maxHp;
        private int _exp;
        private bool _isBoosted;
        private bool _isDelay;
        private bool _isDead;
        private bool _rebootGame;
        private float _boostTime;
        private float _currentRebootTime;
        private PlayerController _myPlayerController;
        private PlayerInput _myPlayerInput;
        private AttackNearestFoes _myAttackNearestFoesComponent;

        private bool _allowedToLevelUp = true;

        private void Awake()
        {
            if (_spells.Length > 0) CurrentSpell = _spells[0];
            Level = 1;
            MaxHP = _baseMaxHP;
            HP = MaxHP;
            EXP = 0;
            Swiftness = 0;
            Constitution = 0;
            Power = 0;
            _myPlayerController = transform.GetComponent<PlayerController>();
            _myPlayerInput = transform.GetComponent<PlayerInput>();
            _myAttackNearestFoesComponent = transform.GetComponent<AttackNearestFoes>();
        }
        
        private void OnEnable()
        {
            OnUpgradeStat += UpgradeStat;
            OnSpeedBoost += SpeedBoost;
        }

        private void OnDisable()
        {
            OnUpgradeStat -= UpgradeStat;
            OnSpeedBoost -= SpeedBoost;
        }

        private void Start()
        {
            ClockGame.Instance.ClockStart();
            StartCoroutine(LoadDataPlayer());
        }

        private void Update()
        {
            //timer for speed boost
            if (_isDelay && _boostTime < _boostDelay) _boostTime += Time.deltaTime;
            if (_isDelay && _boostTime >= _boostDelay)
            {
                _isDelay = false;
                _boostTime = 0;
                Speed /= 2;
                _speedFX.SetActive(false);
                _isBoosted = false;
            }
        }
        
        private IEnumerator LoadDataPlayer()
        {
            DataCollector.OnPlayerSpawned?.Invoke();
            yield return null;
        }

        private void LevelUp()
        {
            if (!_allowedToLevelUp) return;

            if (HP > 0)
            {
                Level++;

                if (Level % 5 == 0 && SpellUnlock < _spells.Length)
                {
                    SpellUnlock++;
                    OnSpellIndexChange?.Invoke(SpellUnlock);
                    UpdateSpell();
                    OnSpellUnlock?.Invoke(CurrentSpell, NextSpell);
                }
                else
                {
                    OnDisplayUpgrade?.Invoke();
                }
            }
        }

        private void UpgradeStat(int indexStat)
        {
            switch (indexStat)
            {
                case 1:
                    Constitution++;
                    MaxHP = _baseMaxHP + (Constitution * _hpPerConst);
                    TakeHeal(_hpPerConst);
                    return;
                case 2:
                    Swiftness++;
                    AttackCooldown += _cooldownUpgrade;
                    return;
                case 3:
                    Power++;
                    SpellPower += Power * SpellPower;
                    return;
            }
        }

        private void SpeedBoost(bool isActive)
        {
            if (isActive && _isBoosted == false)
            {
                Speed *= 2;
                _speedFX.SetActive(true);
                _isBoosted = true;
                return;
            }

            if (isActive && _isBoosted)
            {
                _boostTime = 0;
                return;
            }

            if (!_isDelay) _isDelay = true;
        }

        public void UpdateSpell()
        {
            CurrentSpell = _spells[SpellUnlock];

            NextSpell = SpellUnlock < _spells.Length - 1 ? _spells[SpellUnlock + 1] : null;
        }

        public void TakeDamage(int damage)
        {
            HP -= damage;
            OnHpChanged?.Invoke(HP);

            if (_hp <= 0)
            {
                _allowedToLevelUp = false;
                Death();
            }

            _playerAnimator.SetTrigger(Hurt);
        }

        public void TakeHeal(int amount)
        {
            HP += amount;
            OnHpChanged?.Invoke(HP);
        }

        public void GainEXP(int amount)
        {
            EXP += amount;
            OnExpChanged?.Invoke(EXP);
        }

        private void Death()
        {
            StartCoroutine(DeathAnimationCoroutine());
        }

        private IEnumerator DeathAnimationCoroutine()
        {
            float t = 0f;
            List<Material> materials = new();

            IsDead = true;
            _playerAnimator.SetBool(DeathState, true);
            _myPlayerController.enabled = false;
            _myPlayerInput.enabled = false;
            _myAttackNearestFoesComponent.enabled = false;
            _vfxGraph.Stop();
            foreach (ParticleSystem ps in _particleSystem)
                ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

            ClockGame.Instance.ClockStop();

            yield return GameManager.Instance.CamDeathAnimation();

            while (t < 1)
            {
                t += Time.deltaTime / _deathAnimationDuration;

                foreach (Renderer rd in _renderers)
                {
                    rd.GetMaterials(materials); // Copy originals

                    foreach (Material material in materials) {
                        material.SetFloat(Dissolve, Mathf.Lerp(0, 1, t));
                    }

                    materials.Clear();
                }

                yield return null;
            }

            yield return new WaitForSeconds(.25f);

            OnPlayerDeath?.Invoke();

            Destroy(this.gameObject);
        }
    }
}