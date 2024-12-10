using System;
using Manager;
using Player.AutoAttacks;
using Player.Sort;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Player
{
    public class Character : SingletonMonoBehaviour<Character>
    {
        public static readonly int Hurt = Animator.StringToHash("hurt");
        public static readonly int Death = Animator.StringToHash("death");
        [Header("References")]
        public Transform EnnemyRaycastTarget;
        
        [SerializeField] private Animator _playerAnimator;
        [SerializeField] private GameObject _speedFX;
        
        [Header("SpelldataList")] 
        [SerializeField] private Spell[] Spells;

        [Header("Base Stats")]
        [SerializeField] private int _baseMaxHP;
        [SerializeField] private float _baseSpeed = 500f;
        [SerializeField] private int _baseEXP;
        [SerializeField] private float _baseSpellDamage;
        [SerializeField] private float _attackCooldown = 3;

        [Header("Progression")]
        [SerializeField] private float _cooldownUpgrade = -0.25f;
        [SerializeField] private float _hpGrowthFactor = 0.25f;

        public static Action OnPlayerSpawn;
        public static Action<int> OnHpChanged;
        public static Action<int> OnExpChanged;
        public static Action OnLevelUp;
        public static Action<bool> OnDisplayUpgrade;
        public static Action<int> OnUpgradeStat;
        public static Action<bool> OnSpeedBoost;

        public int Level
        {
            get => _level;
            set {
                _level = value;
                OnLevelUp?.Invoke();
            }
        }
        public int RequireEXP => Mathf.CeilToInt(_baseEXP * Mathf.Pow(Level, 1.5f));
        public int EXP {
            get => _exp;
            set {
                _exp = Mathf.Clamp(value, 0, RequireEXP);
                OnExpChanged?.Invoke(_exp);

                if (_exp >= RequireEXP) {
                    _exp -= RequireEXP;
                    LevelUp();
                }
            }
        }
        public int MaxHP {
            get => _baseMaxHP;
            private set => _baseMaxHP = value;
        }
        public int HP {
            get => _hp;
            set {
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
        public bool IsBoosted { get; private set; }

        private int _level = 1;
        private int _hp;
        private int _exp;
        private bool _isBoosted;
        private bool _isDelay;
        private int _spellUnlock;
        private float _boostTime;
        private float _boostDelay = 3;

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
            if (Spells.Length > 0)
            {
                CurrentSpell = Spells[0];
            }
            Level = 1;
            HP = MaxHP;
            EXP = 0;
            Swiftness = 0;
            Constitution = 0;
            Power = 0;
            
        }

        private void Update()
        {
            //timer for speed boost
            if (_isDelay && _boostTime < _boostDelay)
            {
                _boostTime += Time.deltaTime;
            }
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
            if (Level % 5 == 0) {
                //TODO: Trigger spell evolution
                OnDisplayUpgrade?.Invoke(false);
                _spellUnlock++;
                CurrentSpell = Spells[_spellUnlock];
            }
            else
            {
                // TODO: Trigger stats selection
                OnDisplayUpgrade?.Invoke(true);
            }
        }
        private void UpgradeStat(int indexStat)
        {
            switch (indexStat)
            {
                case 1:
                    Constitution++;
                    MaxHP = (int)(MaxHP*(1 + _hpGrowthFactor));
                    return;
                case 2:
                    Swiftness++;
                    AttackCooldown += _cooldownUpgrade;
                    Debug.Log("attack cooldown:"+AttackCooldown+" vitesse:"+Swiftness);
                    return;
                case 3:
                    Power++;
                    SpellPower *= Power;
                    return;
            }
        }

        private void SpeedBoost(bool isActive)
        {
            if(isActive && _isBoosted == false)
            {
                Speed *= 2;
                _speedFX.SetActive(true);
                _isBoosted = true;
                return;
            }
            if(isActive && _isBoosted)
            {
                _boostTime = 0;
                return;
            }

            if (!_isDelay)
            {
                _isDelay = true;
            }
            
        }
        
        public void TakeDamage(int damage) 
        {
            Debug.Log("Player: damage taken" + damage);
            HP -= damage;
            OnHpChanged?.Invoke(HP);
            if (_hp <= 0) {
                Debug.Log("Player: Dead");
                transform.GetComponent<PlayerController>().enabled = false;
                transform.GetComponent<PlayerInput>().enabled = false;
                transform.GetComponent<AttackNearestFoes>().enabled = false;
                _playerAnimator.SetTrigger(Death);
                //Invoke("SceneLoader.Instance.LaunchGame()",3);
                //TODO: Game Over
                return;
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
        
    }
}