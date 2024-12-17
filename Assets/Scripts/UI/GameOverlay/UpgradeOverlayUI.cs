using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Player;
using Player.Spells_Effects;
using TMPro;
using Utils;

namespace UI.GameOverlay
{
    public class UpgradeOverlayUI : MonoBehaviour
    {
        [Header("Stat Panels")]
        [SerializeField] private CanvasGroup _upgradeStatPanel;
        [SerializeField] private GameObject _attackSpeedPanel;
        [SerializeField] private GameObject _powerPanel;
        [SerializeField] private GameObject _constitutionPanel;

        [Header("Spell Evolution Panels")]
        [SerializeField] private CanvasGroup _spellEvolutionPanel;
        [SerializeField] private TMP_Text _spellName;
        [SerializeField] private Image _currentSpell;
        [SerializeField] private Image _nextSpell;
        [SerializeField] private Button _spellConfirmation;

        [Header("Stats Panels")]
        [SerializeField] private Button _upgradeConstitution;
        [SerializeField] private Button _upgradeSwiftness;
        [SerializeField] private Button _upgradePower;
        [SerializeField] private TMP_Text _constitutionOldValue;
        [SerializeField] private TMP_Text _constitutionNewValue;
        [SerializeField] private TMP_Text _swiftnessOldValue;
        [SerializeField] private TMP_Text _swiftnessNewValue;
        [SerializeField] private TMP_Text _powerOldValue;
        [SerializeField] private TMP_Text _powerNewValue;

        [Header("Settings")]
        [SerializeField] private float _timeWarpDuration = 1.5f;

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
            StartCoroutine(WaitForPlayerUpgradeChoice());
        }

        private void SpellUpgradeDisplay(Spell oldSpell, Spell newSpell)
        {
            StartCoroutine(WaitForPlayerSpellConfirmation(oldSpell, newSpell));
        }

        private IEnumerator WaitForPlayerUpgradeChoice()
        {
            DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0, _timeWarpDuration).SetUpdate(true).SetEase(Ease.InCirc);

            // Update panels
            _constitutionOldValue.text = Character.Instance.Constitution.ToString();
            _constitutionNewValue.text = (Character.Instance.Constitution + 1).ToString();
            _swiftnessOldValue.text = Character.Instance.Swiftness.ToString();
            _swiftnessNewValue.text = (Character.Instance.Swiftness + 1).ToString();
            _powerOldValue.text = Character.Instance.Power.ToString();
            _powerNewValue.text = (Character.Instance.Power + 1).ToString();

            yield return new WaitForSecondsRealtime(1.7f);

            if (Character.Instance.Power == 5)
                _powerPanel.SetActive(false);
            if (Character.Instance.Swiftness == 5)
                _attackSpeedPanel.SetActive(false);
            if (Character.Instance.Constitution == 5)
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
                t += Time.unscaledDeltaTime / _timeWarpDuration;
                Time.timeScale = Mathf.Lerp(0, 1, t);
                yield return null;
            }
            // whenDone.Invoke();
        }

        private IEnumerator WaitForPlayerSpellConfirmation(Spell oldSpell, Spell newSpell)
        {
            DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0, _timeWarpDuration).SetUpdate(true).SetEase(Ease.InCirc);

            yield return new WaitForSecondsRealtime(1.7f);

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
                t += Time.unscaledDeltaTime / _timeWarpDuration;
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