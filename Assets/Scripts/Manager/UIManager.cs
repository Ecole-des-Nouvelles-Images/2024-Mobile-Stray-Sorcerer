using UnityEngine;
using DG.Tweening;

using Player;
using UnityEngine.Serialization;
using Utils;

namespace Manager
{
    public enum ControlSide
    {
        Left,
        Right
    }

    public class UIManager : SingletonMonoBehaviour<UIManager>
    {
        [Header("Settings")]
        [SerializeField] private ControlSide _defaultControlSide = ControlSide.Left;
        [SerializeField] private float _panelSlideDuration = 1.5f;

        [Header("UI Panels")]
        [SerializeField] private CanvasGroup _pauseOverlay;
        [Space(10)]
        [SerializeField] private GameObject _pausePanel;
        [SerializeField] private GameObject _optionsPanel;

        [Header("Joysticks")]
        [SerializeField] private GameObject _joystickL;
        [SerializeField] private GameObject _currentSpellL;
        [Space(5)]
        [SerializeField] private GameObject _joystickR;
        [SerializeField] private GameObject _currentSpellR;

        public ControlSide CurrentControlSide { get; set; }

        public bool InPause { get; private set; } = false;
        public bool InOptions { get; private set; } = false;

        private void Awake()
        {
            CurrentControlSide = _defaultControlSide;
            SwitchJoystickSide(false);
        }

        private void OnEnable()
        {
            PlayerController.OnControlMapChanged += SwitchJoystickSide;
        }

        private void OnDisable()
        {
            PlayerController.OnControlMapChanged -= SwitchJoystickSide;
        }

        public void SwitchPausePanel()
        {
            if (!InPause) {
                Time.timeScale = 0;
                _pauseOverlay.gameObject.SetActive(true);
                _pauseOverlay.DOFade(1, _panelSlideDuration).SetUpdate(true).SetEase(Ease.InOutCubic).OnComplete(() =>
                {
                    _pauseOverlay.interactable = true;
                    _pauseOverlay.blocksRaycasts = true;
                });
                InPause = true;
            }
            else {
                Time.timeScale = 1;
                _pauseOverlay.DOFade(0, _panelSlideDuration).SetUpdate(true).SetEase(Ease.InOutCubic).OnComplete(() =>
                {
                    _pauseOverlay.gameObject.SetActive(false);
                    _pauseOverlay.interactable = false;
                    _pauseOverlay.blocksRaycasts = false;
                });
                InPause = false;
            }
        }

        public void SwitchOptionsPanel()
        {
            if (!InOptions)
            {
                _pausePanel.transform.DOLocalMoveX(-1500f, _panelSlideDuration).SetUpdate(true).SetEase(Ease.InOutCubic);
                _optionsPanel.transform.DOLocalMoveX(0f, _panelSlideDuration).SetUpdate(true).SetEase(Ease.InOutCubic);
                InOptions = true;
            }
            else
            {
                _pausePanel.transform.DOLocalMoveX(0f, _panelSlideDuration).SetUpdate(true).SetEase(Ease.InOutCubic);
                _optionsPanel.transform.DOLocalMoveX(1500f, _panelSlideDuration).SetUpdate(true).SetEase(Ease.InOutCubic);
                InOptions = false;
            }
        }

        private void SwitchJoystickSide(bool state)
        {
            if (CurrentControlSide == ControlSide.Left)
            {
                _joystickL.SetActive(state);
                _currentSpellL.SetActive(!state);
                _joystickR.SetActive(!state);
                _currentSpellR.SetActive(state);
            }
            else
            {
                _joystickR.SetActive(state);
                _currentSpellR.SetActive(!state);
                _joystickL.SetActive(!state);
                _currentSpellL.SetActive(state);
            }
        }
    }
}
