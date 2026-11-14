using DG.Tweening;
using TMPro;
using UnityEngine.Localization.Components;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UI
{
    public class LoadingScreen : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TipsSO _tips;
        [SerializeField] private GameObject _canvas;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TMP_Text _statusInfoBox;
        [SerializeField] private TMP_Text _tipsText;

        [Header("Settings")]
        [SerializeField] private float _fadeDuration = 0.5f;

        private Transform _virtualCamera;
        private float _timerTips;

        private void Awake()
        {
            _virtualCamera = GameObject.Find("Camera/VCam Player").transform;
            if (!_tipsText.GetComponent<LocalizeStringEvent>())
                throw new System.InvalidOperationException(
                    $"The text object '{_tipsText.gameObject.name}' has no LocalizeStringEvent.");
            DisplayRandomTips();
            _timerTips = 10f;
        }

        private void Update()
        {
            if (_timerTips > 0)
                _timerTips -= Time.deltaTime;
            if (_timerTips <= 0)
            {
                DisplayRandomTips();
                _timerTips = 10f;
            }
        }

        private void DisplayRandomTips()
        {
            var tipKey = _tips.TipsList[Random.Range(0, _tips.TipsList.Count)];
            LocalizationText.Set(_tipsText, tipKey);
        }

        public void Show(bool status)
        {
            _virtualCamera.transform.position = new Vector3(0, 1000, -1000);
            _canvasGroup.DOFade(status ? 1 : 0, _fadeDuration).SetUpdate(true);
        }

        public void UpdateLog(string key, params object[] arguments)
        {
            LocalizationText.Set(_statusInfoBox, key, arguments);
        }
    }
}
