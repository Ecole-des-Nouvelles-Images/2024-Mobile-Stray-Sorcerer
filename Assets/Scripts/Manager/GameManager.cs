using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using Cinemachine;
using DG.Tweening;
using Player;
using UI.Effects;
using UnityEngine.Serialization;
using Utils;

namespace Manager
{
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        [Header("References")]
        [SerializeField] private GameObject _ui;
        [SerializeField] private GameObject _playerHUD;
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private TextToParticles _introductionTextMesh;

        [Header("Settings")] [SerializeField] private Vector3 _playerSpawnPosition;
        [SerializeField] private Vector3 _cameraOrientation;
        [SerializeField] private float _cameraFOV = 60;
        [SerializeField] private float _cameraDistance = 25;

        [Header("Death Animation Settings")]
        [SerializeField] private int _deathAnimSpeed = 10;
        [SerializeField] private int _deathAnimMaxDistance = 5;

        private GameObject _player;
        private CinemachineVirtualCamera _camera;
        private CinemachineFramingTransposer _camBody;

        private void Start()
        {
            _camera = GameObject.Find("VCam Player").GetComponent<CinemachineVirtualCamera>();
            _camera.transform.rotation = Quaternion.identity;
        }

        public void StartGame()
        {
            _camera.gameObject.SetActive(true);
            _ui.SetActive(true);

            _player = Instantiate(_playerPrefab, _playerSpawnPosition, Quaternion.identity);
            _player.GetComponent<PlayerInput>().uiInputModule = FindObjectOfType<InputSystemUIInputModule>();
            
            SetupCamera();

            StartCoroutine(IntroductionAnimation());
        }

        private IEnumerator IntroductionAnimation()
        {
            _introductionTextMesh.SpawnText();

            yield return new WaitForSeconds(3);

            _introductionTextMesh.DissolveText();

            yield return new WaitForSeconds(5);

            CanvasGroup uiGroup = _playerHUD.GetComponent<CanvasGroup>();
            uiGroup.DOFade(1, 1);
        }

        public IEnumerator CamDeathAnimation()
        {
            bool animationIsFinished = true;

            if (!_camBody)
                _camBody = _camera.GetCinemachineComponent<CinemachineFramingTransposer>();

            DOTween.To(() => _camBody.m_CameraDistance, x => _camBody.m_CameraDistance = x, _deathAnimMaxDistance, _deathAnimSpeed)
                   .OnComplete(() =>
                   {
                       animationIsFinished = false;
                   });

            while (!animationIsFinished) {
                yield return null;
            }

            /* if(_camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance > _deathAnimMaxDistance)
                _camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance -= Time.deltaTime * _deathAnimSpeed; */
        }

        private void SetupCamera()
        {
            _camBody = _camera.AddCinemachineComponent<CinemachineFramingTransposer>();

            _camera.transform.Rotate(_cameraOrientation);
            _camera.m_Follow = _player.transform;
            _camera.m_Lens.FieldOfView = _cameraFOV;
            _camBody.m_CameraDistance = _cameraDistance;
            _camBody.m_SoftZoneWidth = 0.2f;
            _camBody.m_SoftZoneHeight = 0.2f;
        }
    }
}