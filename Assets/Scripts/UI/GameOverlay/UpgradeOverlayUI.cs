using System;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.GameOverlay
{
    public class UpgradeOverlayUI: MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _upgradeStatPanel;
        [SerializeField] private GameObject _attackSpeedPanel;
        [SerializeField] private GameObject _powerPanel;
        [SerializeField] private GameObject _constitutionPanel;
        [SerializeField] private GameObject _upgradeSpellPanel;
        [SerializeField] private GameObject _currentSpellpanel;
        [SerializeField] private GameObject _nextSpellpanel;

        
        private void OnEnable()
        {
            Character.OnDisplayUpgrade += UpgradeDisplay;
        }

        private void OnDisable()
        {
            Character.OnDisplayUpgrade -= UpgradeDisplay;
        }

        private void Awake()
        {
            _upgradeStatPanel.SetActive(false);
            _upgradeSpellPanel.SetActive(false);
        }
        private void UpgradeDisplay(bool upgradingStats)
        {
            if (upgradingStats)
            {
                _upgradeStatPanel.SetActive(true);
                if(Character.Instance.Power == 5)
                    _powerPanel.SetActive(false);
                if(Character.Instance.Swiftness == 5)
                    _attackSpeedPanel.SetActive(false);
                if(Character.Instance.Constitution == 5)
                    _constitutionPanel.SetActive(false);
                Time.timeScale = 0;
                return;
            }
            _upgradeSpellPanel.SetActive(true);
            _currentSpellpanel.GetComponent<Image>().sprite = Character.Instance.CurrentSpell.spellSprite;
            _nextSpellpanel.GetComponent<Image>().sprite = Character.Instance.NextSpell.spellSprite;
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