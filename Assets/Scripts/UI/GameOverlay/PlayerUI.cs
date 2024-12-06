using System;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayerUI: MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Slider _exp;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private GameObject _upgradeStatPanel;
        [SerializeField] private GameObject _upgradeSpellPanel;

        private void OnEnable()
        {
            Character.OnHpChanged += UpdateHP;
            Character.OnExpChanged += UpdateXP;
            Character.OnLevelUp += UpdateLevelAndStats;
            Character.OnDisplayUpgrade += UpgradeDisplay;
        }

        private void OnDisable()
        {
            Character.OnHpChanged -= UpdateHP;
            Character.OnExpChanged -= UpdateXP;
            Character.OnLevelUp -= UpdateLevelAndStats;
            Character.OnDisplayUpgrade -= UpgradeDisplay;
        }

        private void Awake()
        {
            _upgradeStatPanel.SetActive(false);
            _upgradeSpellPanel.SetActive(false);
        }

        private void UpdateHP(int value)
        {
            // TODO: Manage heart-quarters
        }

        private void UpdateXP(int value)
        {
            _exp.value = value;
        }

        private void UpdateLevelAndStats()
        {
            _level.text = $"Lv. {Character.Instance.Level}";
            _exp.maxValue = Character.Instance.RequireEXP;

            // TODO: Update MaxHealth panel
            // TODO: Update PauseOverlay's statistics panel
        }

        private void UpgradeDisplay(bool upgradingStats)
        {
            if (upgradingStats)
            {
                _upgradeStatPanel.SetActive(true);
                Time.timeScale = 0;
                return;
            }
            _upgradeSpellPanel.SetActive(true);
            Time.timeScale = 0;
        }

        public void UpgradeConst()
        {
            Character.OnUpgradeStat?.Invoke(1);
            _upgradeStatPanel.SetActive(false);
            Time.timeScale = 1;
        }
        public void UpgradeSwiftness()
        {
            Character.OnUpgradeStat?.Invoke(2);
            _upgradeStatPanel.SetActive(false);
            Time.timeScale = 1;
        }
        public void UpgradePower()
        {
            Character.OnUpgradeStat?.Invoke(3);
            _upgradeStatPanel.SetActive(false);
            Time.timeScale = 1;
        }

        public void ValideSpell()
        {
            _upgradeSpellPanel.SetActive(false);
            Time.timeScale = 1;
        }
    }
}