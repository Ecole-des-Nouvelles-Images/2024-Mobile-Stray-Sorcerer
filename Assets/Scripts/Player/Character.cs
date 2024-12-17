using System;
using System.Collections;
using System.Collections.Generic;
using Player.AutoAttacks;
using Player.Spells_Effects;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Player
{
    public class Character : SingletonMonoBehaviour<Character>
    {
        public static readonly int Hurt = Animator.StringToHash("hurt");
        public static readonly int Death = Animator.StringToHash("isDead");
        public static readonly int Dissolve = Shader.PropertyToID("_State");

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

        [Header("Timers")]
        [SerializeField] private float _rebootDelay = 3;
        [SerializeField] private float _boostDelay = 3;
        [SerializeField] private float _deathAnimationDuration = 3;

        public static Action OnPlayerSpawn;
        public static Action<int> OnHpChanged;
        public static Action<int> OnMaxHpChanged;
        public static Action<int> OnExpChanged;
        public static Action OnLevelUp;
        public static Action<Spell, Spell> OnSpellUnlock;
        public static Action OnDisplayUpgrade;
        public static Action<int> OnUpgradeStat;
        public static Action<bool> OnSpeedBoost;

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
                OnExpChanged?.Invoke(_exp);

                if (_exp >= RequireEXP)
                {
                    _exp -= RequireEXP;
                    LevelUp();
                }
            }
        }
        public int MaxHP
        {
            get => _maxHp;
            private set
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

        public int Constitution { get; private set; }
        public int Swiftness { get; private set; }
        public int Power { get; private set; }

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
        private int _spellUnlock;
        private float _boostTime;
        private float _currentRebootTime;
        private PlayerController _myPlayerController;
        private PlayerInput _myPlayerInput;
        private AttackNearestFoes _myAttackNearestFoesComponent;

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
            _currentRebootTime = _rebootDelay;
            _myPlayerController = transform.GetComponent<PlayerController>();
            _myPlayerInput = transform.GetComponent<PlayerInput>();
            _myAttackNearestFoesComponent = transform.GetComponent<AttackNearestFoes>();
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

        private void LevelUp()
        {
            Level++;

            if (Level % 5 == 0 && _spellUnlock < _spells.Length)
            {
                _spellUnlock++;
                CurrentSpell = _spells[_spellUnlock];

                NextSpell = _spellUnlock < _spells.Length - 1 ? _spells[_spellUnlock + 1] : null;

                OnSpellUnlock?.Invoke(CurrentSpell, NextSpell);
            }
            else
            {
                OnDisplayUpgrade?.Invoke();
            }
        }

        private void UpgradeStat(int indexStat)
        {
            switch (indexStat)
            {
                case 1:
                    Constitution++;
                    MaxHP = _baseMaxHP + (Constitution * 2);
                    TakeHeal(2);
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

        private void PlayerSpawn()
        {
            _playerAnimator.SetBool(Death, false);
            HP = MaxHP;
            transform.position = new Vector3(0, 0, 0);
            _myPlayerController.enabled = true;
            _myPlayerInput.enabled = true;
            _myAttackNearestFoesComponent.enabled = true;
            _currentRebootTime = _rebootDelay;
            IsDead = false;

            OnPlayerSpawn?.Invoke();
        }

        public void TakeDamage(int damage)
        {
            HP -= damage;
            OnHpChanged?.Invoke(HP);

            if (_hp <= 0)
            {
                StartCoroutine(DeathAnimationCoroutine());
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

        private IEnumerator DeathAnimationCoroutine()
        {
            float t = 0f;
            List<Material> materials = new();

            IsDead = true;
            _playerAnimator.SetBool(Death, true);
            _myPlayerController.enabled = false;
            _myAttackNearestFoesComponent.enabled = false;

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

            yield return new WaitForSeconds(1.5f);

            PlayerSpawn();
        }
    }
}