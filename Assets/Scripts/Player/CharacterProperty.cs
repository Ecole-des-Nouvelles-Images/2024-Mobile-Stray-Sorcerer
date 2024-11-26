using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Player
{
    public class CharacterProperty : MonoBehaviour {
        public int RequireEXP => Mathf.CeilToInt(basicMaxExperienceValue * Mathf.Pow(_currentCharacterLevel, 1.5f));
        [Header("Basic Character Properties")]
        [SerializeField] private int basicMaxHealPointValue;
        [SerializeField] private int basicMaxExperienceValue;
        [SerializeField] private int basicSpeedValue;
        [SerializeField] private int basicPowerValue;
        [Header("UI")]
        [SerializeField] private Image characterCurrentHPDisplay;

        private int _currentCharacterLevel;
        private int _currentMaxHealPointValue;
        private int _currentHealPointValue;
        private int _currentExperienceValue;
        private int _currentSpeedValue;
        private int _currentPowerValue;
        private int _ConstitutionUpgradeCounter;
        private int _SpeedUpgradeCounter;
        private int _PowerUpgradeCounter;
        private void Start() {
            InitializeCharacterProperty();
        }

        private void Update() {
            characterCurrentHPDisplay.fillAmount = Helper.LoadFactorCalculation(_currentHealPointValue,_currentMaxHealPointValue);
        }

        private void UpgradeCharacter() {
            _currentCharacterLevel++;
            if (_currentCharacterLevel % 5 != 0) {
                //Display stats to upgrade
            }
            else {
                //Display spell upgrade
            }
        }

        public void InitializeCharacterProperty() {
            _currentCharacterLevel = 1;
            _currentMaxHealPointValue = basicMaxHealPointValue;
            _currentHealPointValue = basicMaxHealPointValue;
            _currentExperienceValue = 0;
            _currentSpeedValue = basicSpeedValue;
            _currentPowerValue = basicPowerValue;
        }
        
        public void TakeDamage(int damage) {
            _currentHealPointValue -= damage;
            if (_currentHealPointValue < 0) _currentHealPointValue = 0;
        }

        public void TakeHeal(int heal) {
            _currentHealPointValue += heal;
            if (_currentHealPointValue > _currentMaxHealPointValue) _currentHealPointValue = _currentMaxHealPointValue;
        }

        public void GainExperience(int experience) {
            _currentExperienceValue += experience;
            if (_currentExperienceValue >= RequireEXP) {
                UpgradeCharacter();
                _currentExperienceValue = 0;
            }
        }
    }
}
