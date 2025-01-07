using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI
{
    public class LoadingScreen : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _canvas;

        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TMP_Text _statusInfoBox;

        [Header("Settings")]
        [SerializeField] private float _fadeDuration = 0.5f;

        [Header("Particle System")]
        [SerializeField] private GameObject _psRoot;


        public void Show(bool status)
        {
            _canvasGroup.DOFade(status ? 1 : 0, _fadeDuration).SetUpdate(true);
            _psRoot.gameObject.SetActive(status);
        }

        public void UpdateStatus(string status)
        {
            // Disable the method
            return;

            _statusInfoBox.text = status;
        }
    }
}