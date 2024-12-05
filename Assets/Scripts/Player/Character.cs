using System;
using Player.Sort;
using UnityEngine;
using Utils;

namespace Player
{
    public class Character : SingletonMonoBehaviour<Character>
    {
        [Header("SpelldataList")] 
        public Spell[] Spells;

        [Header("Base Stats")]
        [SerializeField] private int _baseMaxHP;
        [SerializeField] private float _baseSpeed = 500f;
        [SerializeField] private int _baseEXP;
        [SerializeField] private int _baseSpellDamage;

        [Header("Progression")]
        [SerializeField] private float _speedGrowthFactor = 0.1f;
        [SerializeField] private float _spellDamageGrowthFactor = 0.1f;
        
        public static Action<int> OnHpChanged;
        public static Action<int> OnExpChanged;
        public static Action OnLevelUp;
        public static Action<bool> OnDisplayUpgrade;
        public static Action<int> OnUpgradeStat;

        private void OnEnable()
        {
            OnUpgradeStat += UpgradeStat;
        }

        private void OnDisable()
        {
            OnUpgradeStat -= UpgradeStat;
        }

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
        public int MaxHP => _baseMaxHP + Constitution;
        public int HP {
            get => _hp;
            set {
                _hp = Mathf.Clamp(value, 0, MaxHP);
                OnHpChanged?.Invoke(_hp);
            }
        }
        public float Speed => _baseSpeed * (1 + Swiftness * _speedGrowthFactor);
        public float SpellPower => _baseSpellDamage * (1 + Power * _spellDamageGrowthFactor);
        public int Constitution { get; private set; }
        public int Swiftness { get; private set; }
        public int Power { get; private set; }
        public Spell CurrentSpell { get; private set; }

        private int _level = 1;
        private int _hp;
        private int _exp;
        
        private int _spellUnlock;
        

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

        public void TakeDamage(int damage) 
        {
            HP -= damage;
            OnHpChanged?.Invoke(HP);

            if (_hp <= 0) {
                //TODO: Game Over
            }
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

        private void UpgradeStat(int indexStat)
        {
            switch (indexStat)
            {
                case 1:
                    Constitution++;
                    return;
                case 2:
                    Swiftness++;
                    return;
                case 3:
                    Power++;
                    return;
            }
        }
    }
}