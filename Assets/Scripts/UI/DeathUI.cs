using DG.Tweening;
using Gameplay;
using Manager;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DeathUI : EndGameUI
    {
        [Header("Death Display")]
        [SerializeField] private GameObject _deathDisplay;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private CanvasGroup _fader;
        [SerializeField] private Button _confirm;
        [SerializeField] float _fadeDuration = 1f;

        private void OnEnable()
        {
            Character.OnPlayerDeath += SelfActivation;

            _confirm.enabled = true;
            _confirm.interactable = true;
        }

        private void OnDisable()
        {
            Character.OnPlayerDeath -= SelfActivation;
        }
        private void OnDestroy()
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
                EnableUIInputActions();
                _confirm.Select();
            });
        }

        public new void ReturnTitle()
        {
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;

            ClockGame.Instance.Reset();

            _canvasGroup.DOFade(0, _fadeDuration).SetUpdate(true);
            _fader.DOFade(1, _fadeDuration).SetUpdate(true).OnComplete(() =>
            {
                Time.timeScale = 1;
                SceneLoader.Instance.LoadTitleScreen();
            });
        }
    }
}
