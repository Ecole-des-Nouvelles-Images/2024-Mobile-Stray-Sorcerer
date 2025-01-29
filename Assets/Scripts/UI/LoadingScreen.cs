using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UI
{
    public class LoadingScreen : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TipsSO _Tips;
        [SerializeField] private GameObject _canvas;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TMP_Text _statusInfoBox;
        [SerializeField] private TMP_Text _tipsText;

        [Header("Settings")]
        [SerializeField] private float _fadeDuration = 0.5f;

        [Header("Particle System")]
        [SerializeField] private GameObject _psRoot;

        private Transform _virtualCamera;
        private float _timerTips;

        private void Awake()
        {
            _virtualCamera = GameObject.Find("Camera/VCam Player").transform;
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
            _tipsText.text = _Tips.TipsList[Random.Range(0, _Tips.TipsList.Count - 1)];
        }

        public void Show(bool status)
        {
            _virtualCamera.transform.position = new Vector3(0, 1000, -1000);
            _canvasGroup.DOFade(status ? 1 : 0, _fadeDuration).SetUpdate(true);
            _psRoot.gameObject.SetActive(status);
        }
        
    }
}