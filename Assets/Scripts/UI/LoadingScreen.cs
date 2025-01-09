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

        private Transform _virtualCamera;

        private void Awake()
        {
            _virtualCamera = GameObject.Find("Camera/VCam Player").transform;
        }

        public void Show(bool status)
        {
            _virtualCamera.transform.position = new Vector3(0, 1000, -1000);
            _canvasGroup.DOFade(status ? 1 : 0, _fadeDuration).SetUpdate(true);
            _psRoot.gameObject.SetActive(status);
        }
    }
}