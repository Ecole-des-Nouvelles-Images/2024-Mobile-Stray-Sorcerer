using UnityEngine;

namespace Christopher.C_Scripts.Player
{
    public class CharacterProperty : MonoBehaviour {
        public int RequireEXP => Mathf.CeilToInt(basicMaxExperienceValue * Mathf.Pow(_currentCharacterLevel, 1.5f));
        [Header("Basic Character Properties")]
        [SerializeField] private int basicMaxHealPointValue;
        [SerializeField] private int basicMaxExperienceValue;
        [SerializeField] private int basicSpeedValue;
        [SerializeField] private int basicPowerValue;
        [Header("Basic Character Properties")]
        [SerializeField] private GameObject characterCurrentHPDisplay;

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
            _currentCharacterLevel = 1;
            _currentMaxHealPointValue = basicMaxHealPointValue;
            _currentHealPointValue = basicMaxHealPointValue;
            _currentExperienceValue = 0;
            _currentSpeedValue = basicSpeedValue;
            _currentPowerValue = basicPowerValue;
        }

        void Update()
        {
            
        }
        
    }
}
