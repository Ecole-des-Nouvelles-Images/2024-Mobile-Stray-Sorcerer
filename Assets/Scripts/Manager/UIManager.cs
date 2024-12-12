using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

using Utils;

namespace Manager
{
    [Serializable]
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
        [SerializeField] private Toggle _joystickOptionL;
        [SerializeField] private Toggle _joystickOptionR;
        [Space(5)]
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
            SwitchJoystickSide();
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

        public void SwitchJoystickSide()
        {
            if (CurrentControlSide == ControlSide.Right)
            {
                _joystickOptionL.interactable = false;
                _joystickOptionR.interactable = true;
                _joystickOptionR.isOn = false;
                _joystickL.SetActive(true);
                _currentSpellL.SetActive(false);
                _joystickR.SetActive(false);
                _currentSpellR.SetActive(true);
                CurrentControlSide = ControlSide.Left;
            }
            else
            {
                _joystickOptionR.interactable = false;
                _joystickOptionL.interactable = true;
                _joystickOptionL.isOn = false;
                _joystickR.SetActive(true);
                _currentSpellR.SetActive(false);
                _joystickL.SetActive(false);
                _currentSpellL.SetActive(true);
                CurrentControlSide = ControlSide.Right;
            }
        }
    }
}
