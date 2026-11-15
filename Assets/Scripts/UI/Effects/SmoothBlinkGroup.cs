using DG.Tweening;
using UnityEngine;

namespace UI.Effects
{
    [RequireComponent(typeof(CanvasGroup))]
    public class SmoothBlinkGroup : MonoBehaviour
    {
        [SerializeField] private float _duration = 1f;

        private CanvasGroup _group;
        private float _current;

        private Tween _blink;

        private void Awake()
        {
            _group = GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            _group.alpha = 0;
            _blink = _group.DOFade(1, _duration).SetUpdate(true).SetLoops(-1, LoopType.Yoyo);
        }

        private void OnDisable()
        {
            _blink.Kill(true);
        }
    }
}
