using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LoadingScreen : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private GameObject _canvas;

        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Image _animatedIcon;
        [SerializeField] private TMP_Text _statusInfoBox;
        [SerializeField] private TMP_Text _tipsTextBox;

        [Header("Settings")] [SerializeField] private float _fadeDuration = 0.5f;
        [SerializeField] private Vector2 _iconScaleRange = new(0, 1);
        [SerializeField] private float _iconAnimationScaleDuration = 1f;
        [SerializeField] private float _iconAnimationColorDuration = 1f;
        [SerializeField] private Gradient _iconColorGradient;

        public void Show(bool status)
        {
            _canvas.SetActive(status);
            StartCoroutine(FadeCoroutine(status ? 1 : 0));
        }

        public void UpdateStatus(string status)
        {
            _statusInfoBox.text = status;
        }

        private IEnumerator FadeCoroutine(float targetAlpha)
        {
            float t = 0f;
            float initialAlpha = _canvasGroup.alpha;

            while (t < 1)
            {
                t += Time.deltaTime / _fadeDuration;
                _canvasGroup.alpha = Mathf.Lerp(initialAlpha, targetAlpha, t);
                yield return null;
            }

            // Shows icon by setting base max scale
            _animatedIcon.transform.localScale = _iconScaleRange.y * Vector3.one;

            if (targetAlpha == 0)
            {
                _animatedIcon.DOKill();
            }
            else
            {
                _animatedIcon.transform.DOScale(Vector3.one * _iconScaleRange.x, _iconAnimationScaleDuration).SetLoops(-1, LoopType.Yoyo);
                _animatedIcon.DOGradientColor(_iconColorGradient, _iconAnimationColorDuration).SetLoops(-1, LoopType.Yoyo);
            }
        }
    }
}