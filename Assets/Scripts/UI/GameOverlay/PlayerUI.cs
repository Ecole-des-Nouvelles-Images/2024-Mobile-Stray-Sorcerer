using System;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameOverlay
{
    public class PlayerUI: MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _upgradeStatPanel;
        [SerializeField] private GameObject _attackSpeedPanel;
        [SerializeField] private GameObject _powerPanel;
        [SerializeField] private GameObject _upgradeSpellPanel;
        [SerializeField] private GameObject[] _spellDisplayPanels;
        [SerializeField] private GameObject _hpPanel;

        public static Action OnSpellSpriteUpdate;
        
        private void OnEnable()
        {
            Character.OnHpChanged += UpdateHP;
            Character.OnDisplayUpgrade += UpgradeDisplay;
        }

        private void OnDisable()
        {
            Character.OnHpChanged -= UpdateHP;
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
                    _hpPanel.SetActive(false);
                Time.timeScale = 0;
                return;
            }
            _upgradeSpellPanel.SetActive(true);
            Time.timeScale = 0;
        }

        private void UpdateSpellSprite()
        {
            for (int i = 0; i < -_spellDisplayPanels.Length; i++)
            {
                _spellDisplayPanels[i].gameObject.GetComponent<Image>().sprite = Character.Instance.CurrentSpell.spellSprite;
            }
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