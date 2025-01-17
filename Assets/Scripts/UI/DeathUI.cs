﻿using DG.Tweening;
using Gameplay.GameData;
using Manager;
using Player;
using UnityEngine;

namespace UI
{
    public class DeathUI : EndGameUI
    {
        [Header("Death Display")]
        [SerializeField] private GameObject _deathDisplay;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private CanvasGroup _fader;
        [SerializeField] float _fadeDuration = 1f;

        private void OnEnable()
        {
            Character.OnPlayerDeath += SelfActivation;
        }

        private void OnDisable()
        {
            Character.OnPlayerDeath -= SelfActivation;
        }

        private void Start()
        {
            _canvasGroup.alpha = 0;
        }

        private void SelfActivation()
        {
            Time.timeScale = 0;

            _canvasGroup.DOFade(1, _fadeDuration).SetUpdate(true).OnComplete(() =>
            {
                _canvasGroup.interactable = true;
                _canvasGroup.blocksRaycasts = true;
            });

            DataCollector.Instance.Death();
            UpdateDisplay();
        }

        public void ReturnTitle()
        {
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;

            ClockGame.Instance.Reset();

            _canvasGroup.DOFade(0, _fadeDuration).SetUpdate(true);
            _fader.DOFade(1, _fadeDuration).SetUpdate(true).OnComplete(() =>
            {
                Time.timeScale = 1;
                DataCollector.Instance.ResetSave();
                SceneLoader.Instance.LoadTitleScreen();
            });
        }
    }
}