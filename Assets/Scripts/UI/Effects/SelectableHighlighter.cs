using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Effects
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Selectable))]
    public class SelectableHighlighter : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [Header("Clignotement")]
        [SerializeField] private float _blinkDuration = 2f; // Durée d'un cycle de clignotement
        [SerializeField] private float _HDRIntensityFactor = 10f; // Facteur d'intensité HDR (1.0 = original, >1.0 = plus lumineux)
        [SerializeField] private Ease _blinkEase = Ease.InOutSine;

        private Selectable _selectable;
        private Graphic _graphic;
        private Color _originalColor;
        private Tween _blinkTween;

        private void Awake()
        {
            _selectable = GetComponent<Selectable>();
            _graphic = GetComponent<Graphic>();

            if (_graphic)
                _originalColor = _graphic.color;
        }

        private void OnDisable()
        {
            _blinkTween?.Kill();
        }

        public void OnSelect(BaseEventData eventData)
        {
            if (!_graphic) return;

            _blinkTween?.Kill();

            // Crée la couleur HDR cible
            Color targetColor = LightenColor(_originalColor, _HDRIntensityFactor);

            // Anime entre la couleur originale et la couleur HDR
            _blinkTween = DOVirtual.Float(
                    0f, 1f, _blinkDuration,
                    value => _graphic.color = Color.Lerp(_originalColor, targetColor, value)
                )
                .SetEase(_blinkEase)
                .SetLoops(-1, LoopType.Yoyo);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            _blinkTween?.Kill();
            if (_graphic)
                _graphic.color = _originalColor;
        }

        private Color LightenColor(Color color, float intensityFactor)
        {
            return new Color(
                color.r * intensityFactor,
                color.g * intensityFactor,
                color.b * intensityFactor,
                color.a
            );
        }
    }
}
