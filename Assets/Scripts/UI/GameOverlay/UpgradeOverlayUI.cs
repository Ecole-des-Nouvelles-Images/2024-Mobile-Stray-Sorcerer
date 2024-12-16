using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

using Player;
using Player.Spells_Effects;
using UnityEngine.Serialization;
using Utils;

namespace UI.GameOverlay
{
    public class UpgradeOverlayUI: MonoBehaviour
    {
        [Header("Stat Panels")]
        [SerializeField] private CanvasGroup _upgradeStatPanel;
        [SerializeField] private GameObject _attackSpeedPanel;
        [SerializeField] private GameObject _powerPanel;
        [SerializeField] private GameObject _constitutionPanel;

        [Header("Spell Evolution Panels")]
        [SerializeField] private CanvasGroup _spellEvolutionPanel;
        [SerializeField] private Image _currentSpell;
        [SerializeField] private Image _nextSpell;
        [SerializeField] private Button _spellConfirmation;

        [Header("Stats Buttons")]
        [SerializeField] private Button _upgradeConstitution;
        [SerializeField] private Button _upgradeSwiftness;
        [SerializeField] private Button _upgradePower;

        private WaitForUIButtons _waitPlayerChoice;
        private WaitForUIButtons _waitPlayerConfirmation;

        private void Awake()
        {
            _waitPlayerChoice = new WaitForUIButtons(_upgradeConstitution, _upgradeSwiftness, _upgradePower);

            _upgradeStatPanel.interactable = false;
            _upgradeStatPanel.blocksRaycasts = false;
            _upgradeStatPanel.alpha = 0;

            _spellEvolutionPanel.interactable = false;
            _spellEvolutionPanel.blocksRaycasts = false;
            _spellEvolutionPanel.alpha = 0;
        }
        
        private void OnEnable()
        {
            Character.OnDisplayUpgrade += UpgradeDisplay;
            Character.OnSpellUnlock += SpellUpgradeDisplay;
        }

        private void OnDisable()
        {
            Character.OnDisplayUpgrade -= UpgradeDisplay;
            Character.OnSpellUnlock -= SpellUpgradeDisplay;
        }

        private void UpgradeDisplay()
        {
            StartCoroutine(WaitForPlayerUpgradeChoice(() =>
            {
                // Time.timeScale = 1;
                // DOTween.To(()=> Time.timeScale, x=> Time.timeScale = x, 0, 2f).SetUpdate(true);
            }));
        }

        private void SpellUpgradeDisplay(Spell oldSpell, Spell newSpell)
        {
            StartCoroutine(WaitForPlayerSpellConfirmation(oldSpell, newSpell, () =>
            {
                // Time.timeScale = 1;
                // DOTween.To(()=> Time.timeScale, x=> Time.timeScale = x, 0, 2f).SetUpdate(true);
            }));
        }

        private IEnumerator WaitForPlayerUpgradeChoice(Action whenDone)
        {
            DOTween.To(()=> Time.timeScale, x=> Time.timeScale = x, 0, 2f).SetUpdate(true);

            yield return new WaitForSecondsRealtime(2f);

            if(Character.Instance.Power == 5)
                _powerPanel.SetActive(false);
            if(Character.Instance.Swiftness == 5)
                _attackSpeedPanel.SetActive(false);
            if(Character.Instance.Constitution == 5)
                _constitutionPanel.SetActive(false);

            _upgradeStatPanel.DOFade(1, 0.5f).SetUpdate(true);
            _upgradeStatPanel.interactable = true;
            _upgradeStatPanel.blocksRaycasts = true;

            yield return _waitPlayerChoice.Reset();

            if (_waitPlayerChoice.PressedButton == _upgradeConstitution)
                UpgradeConst();
            else if (_waitPlayerChoice.PressedButton == _upgradeSwiftness)
                UpgradeSwiftness();
            else if (_waitPlayerChoice.PressedButton == _upgradePower)
                UpgradePower();

            _upgradeStatPanel.interactable = false;
            _upgradeStatPanel.blocksRaycasts = false;
            _upgradeStatPanel.DOFade(0, 0.5f).SetUpdate(true);

            float t = 0f;

            while (t < 1)
            {
                t += Time.unscaledDeltaTime / 3f;
                Time.timeScale = Mathf.Lerp(0, 1, t);
                yield return null;
            }
            // whenDone.Invoke();
        }

        private IEnumerator WaitForPlayerSpellConfirmation(Spell oldSpell, Spell newSpell, Action whenDone)
        {
            DOTween.To(()=> Time.timeScale, x=> Time.timeScale = x, 0, 2f).SetUpdate(true);

            yield return new WaitForSecondsRealtime(2.5f);

            _currentSpell.sprite = oldSpell.spellSprite;
            _nextSpell.sprite = newSpell.spellSprite;

            _spellEvolutionPanel.DOFade(1, 0.5f).SetUpdate(true);
            _spellEvolutionPanel.interactable = true;
            _spellEvolutionPanel.blocksRaycasts = true;

            yield return _waitPlayerConfirmation.Reset();

            _spellEvolutionPanel.interactable = false;
            _spellEvolutionPanel.blocksRaycasts = false;
            _spellEvolutionPanel.DOFade(0, 0.5f).SetUpdate(true);

            float t = 0f;

            while (t < 1)
            {
                t += Time.unscaledDeltaTime / 2f;
                Time.timeScale = Mathf.Lerp(0, 1, t);
                yield return null;
            }
            // whenDone.Invoke();
        }

        public void UpgradeConst()
        {
            Character.OnUpgradeStat?.Invoke(1);
        }
        public void UpgradeSwiftness()
        {
            Character.OnUpgradeStat?.Invoke(2);
        }
        public void UpgradePower()
        {
            Character.OnUpgradeStat?.Invoke(3);
        }
    }
}