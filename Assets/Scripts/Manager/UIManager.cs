using System;
using Audio;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Gameplay.GameData;
using Player;
using Utils;

using ClockGame = Gameplay.GameData.ClockGame;

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
        [SerializeField] private GameObject[] _spellDisplayPanels;

        [Header("UI Panels")]
        [SerializeField] private CanvasGroup _pauseOverlay;
        [SerializeField] private GameObject _pausePanel;
        [SerializeField] private GameObject _optionsPanel;

        [Header("Sliders")]
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _SFXSlider;
        [SerializeField] private Slider _luminositySlider;

        [Header("Buttons")]
        [SerializeField] private Button _optionsReturnButton;
        [SerializeField] private Toggle _joystickOptionL;
        [SerializeField] private Toggle _joystickOptionR;

        [Header("Joysticks")]
        [SerializeField] private GameObject _joystickL;
        [SerializeField] private GameObject _joystickR;
        [SerializeField] private GameObject _currentSpellL;
        [SerializeField] private GameObject _currentSpellR;

        [Header("Transition")]
        [SerializeField] private CanvasGroup _fader;

        public ControlSide CurrentControlSide { get; set; }

        public bool InPause { get; private set; }
        public bool InOptions { get; private set; }

        private bool _isLeftJoystick = true;

        private void Start()
        {
            LoadSettingsData();

            CurrentControlSide = _defaultControlSide;
        }

        private void InitJoystick()
        {
            if (_isLeftJoystick)
            {
                _joystickR.SetActive(false);
                _joystickL.SetActive(true);

                _joystickOptionL.interactable = false;
                _joystickOptionR.interactable = true;
                _joystickOptionR.isOn = false;
                // _currentSpellL.SetActive(false);
                // _currentSpellR.SetActive(true);
                CurrentControlSide = ControlSide.Left;
            }
            else
            {
                _joystickL.SetActive(false);
                _joystickR.SetActive(true);

                _joystickOptionR.interactable = false;
                _joystickOptionL.interactable = true;
                _joystickOptionL.isOn = false;
                // _currentSpellR.SetActive(false);
                // _currentSpellL.SetActive(true);
                CurrentControlSide = ControlSide.Right;
            }
        }

        private void LoadSettingsData()
        {
            try
            {
                DataCollector.Instance.LoadSettings(_isLeftJoystick, _musicSlider.value, _SFXSlider.value, _luminositySlider.value);
                _isLeftJoystick = DataCollector.Instance.IsLeftJoystick;
                _musicSlider.value = DataCollector.Instance.MusicSlider;
                _SFXSlider.value = DataCollector.Instance.SfxSlider;
                _luminositySlider.value = DataCollector.Instance.LuminositySlider;
            }
            catch
            {
                _luminositySlider.maxValue = 2;
                _luminositySlider.minValue = 0;
                _luminositySlider.value = 1f;
                _musicSlider.value = 0.5f;
                _SFXSlider.value = 0.5f;
            }

            InitJoystick();
            UpdateLuminosity();
            UpdateSFXVolume();
            UpdateMusicVolume();
        }

        public void SwitchPausePanel()
        {
            if (!InPause)
            {
                Time.timeScale = 0;
                _pauseOverlay.gameObject.SetActive(true);
                _pauseOverlay.DOFade(1, _panelSlideDuration).SetUpdate(true).SetEase(Ease.InOutCubic).OnComplete(() =>
                {
                    _pauseOverlay.interactable = true;
                    _pauseOverlay.blocksRaycasts = true;
                });
                InPause = true;
            }
            else
            {
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
                _optionsReturnButton.interactable = true;
                _pausePanel.transform.DOLocalMoveX(-1500f, _panelSlideDuration).SetUpdate(true).SetEase(Ease.InOutCubic);
                _optionsPanel.transform.DOLocalMoveX(0f, _panelSlideDuration).SetUpdate(true).SetEase(Ease.InOutCubic);
                InOptions = true;
            }
            else
            {
                _optionsReturnButton.interactable = false;
                _pausePanel.transform.DOLocalMoveX(0f, _panelSlideDuration).SetUpdate(true).SetEase(Ease.InOutCubic);
                _optionsPanel.transform.DOLocalMoveX(1500f, _panelSlideDuration).SetUpdate(true).SetEase(Ease.InOutCubic);
                InOptions = false;
            }
        }

        public void SwitchJoystickSide()
        {
            if (CurrentControlSide == ControlSide.Right)
            {
                _isLeftJoystick = true;
                _joystickR.SetActive(false);
                _joystickL.SetActive(true);

                _joystickOptionL.interactable = false;
                _joystickOptionR.interactable = true;
                _joystickOptionR.isOn = false;
                // _currentSpellL.SetActive(false);
                // _currentSpellR.SetActive(true);
                CurrentControlSide = ControlSide.Left;
            }
            else
            {
                _isLeftJoystick = false;
                _joystickL.SetActive(false);
                _joystickR.SetActive(true);

                _joystickOptionR.interactable = false;
                _joystickOptionL.interactable = true;
                _joystickOptionL.isOn = false;
                // _currentSpellR.SetActive(false);
                // _currentSpellL.SetActive(true);
                CurrentControlSide = ControlSide.Right;
            }
        }

        public void UpdateLuminosity()
        {
            float value = _luminositySlider.value;
            RenderSettings.ambientIntensity = value;
        }

        public void UpdateMusicVolume()
        {
            float value = _musicSlider.value;
            AudioManager.Instance.UpdateMusicVolume(value);
        }

        public void UpdateSFXVolume()
        {
            float value = _SFXSlider.value;
            AudioManager.Instance.UpdateSFXVolume(value);
        }

        public void SaveModifications()
        {
            DataCollector.Instance.SaveSettings(_isLeftJoystick,_musicSlider.value,_SFXSlider.value,_luminositySlider.value);
        }

        public void ReloadGame()
        {
            Destroy(Character.Instance.gameObject);
            DataCollector.Instance.ResetSave();
            SceneLoader.Instance.ReloadGameScene();
            ClockGame.Instance.Reset();
        }

        public void ReturnToTitle()
        {
            ClockGame.Instance.Reset();

            _fader.DOFade(1, 1.5f).SetUpdate(true).OnComplete(() =>
            {
                Time.timeScale = 1;
                DataCollector.Instance.ResetSave();
                SceneLoader.Instance.LoadTitleScreen();
            });
        }
    }
}