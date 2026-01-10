using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using Unity.Cinemachine;
using DG.Tweening;
using Player;
using UI.Effects;
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

        [Header("Settings")]
        [SerializeField] private Vector3 _playerSpawnPosition;
        [SerializeField] private Vector3 _cameraOrientation;
        [SerializeField] private float _cameraFOV = 60;
        [SerializeField] private float _cameraDistance = 25;

        [Header("Death Animation Settings")]
        [SerializeField] private int _deathAnimSpeed = 10;
        [SerializeField] private int _deathAnimMaxDistance = 5;

        [Header("Introduction animation settings")]
        [SerializeField] private float _introDisplayDuration = 3;
        [SerializeField] private float _introDissolveDuration = 5;

        private GameObject _player;
        private CinemachineCamera _camera;
        private CinemachinePositionComposer _camBody;

        public static Action OnGameStart;

        private void Start()
        {
            _camera = GameObject.Find("VCam Player").GetComponent<CinemachineCamera>();
            _camera.transform.rotation = Quaternion.identity;
        }

        public void StartGame()
        {
            _camera.gameObject.SetActive(true);
            _ui.SetActive(true);
            UIManager.Instance.gameObject.SetActive(true);

            _player = SceneLoader.Instance.SceneUtilityActivatePlayer(_playerPrefab, _playerSpawnPosition);
            _player.GetComponent<PlayerInput>().uiInputModule = FindFirstObjectByType<InputSystemUIInputModule>(FindObjectsInactive.Include);

            SetupCamera();


            StartCoroutine(IntroductionAnimation());
        }

        private IEnumerator IntroductionAnimation()
        {
            _introductionTextMesh.SpawnText();

            yield return new WaitForSeconds(_introDisplayDuration);

            _introductionTextMesh.DissolveText();

            yield return new WaitForSeconds(_introDissolveDuration);

            CanvasGroup uiGroup = _playerHUD.GetComponent<CanvasGroup>();
            uiGroup.DOFade(1, 1);

            yield return new WaitForSeconds(2);

            Destroy(GameObject.Find("UI/GameOverlay/IntroRenderTex"));
            Destroy(GameObject.Find("IntroductionText"));
        }

        public IEnumerator CamDeathAnimation()
        {
            if (!_camBody)
                _camBody = _camera.GetCinemachineComponent(CinemachineCore.Stage.Body) as CinemachinePositionComposer;

            DOTween.To(() => _camBody.CameraDistance, x => _camBody.CameraDistance = x, _deathAnimMaxDistance, _deathAnimSpeed);

            yield return new WaitForSeconds(_deathAnimSpeed + 0.5f);
        }

        private void SetupCamera()
        {
            _camBody = _camera.GetCinemachineComponent(CinemachineCore.Stage.Body) as CinemachinePositionComposer;

            if (!_camBody)
                throw new NullReferenceException("[GameManager] Cinemachine body stage (CinemachinePositionComposer) is not set.");

            _camera.transform.Rotate(_cameraOrientation);
            _camera.Target = new CameraTarget { TrackingTarget = _player.transform };
            _camera.Lens.FieldOfView = _cameraFOV;
            _camBody.CameraDistance = _cameraDistance;
            _camBody.Composition.HardLimits.Size = new Vector2(0.2f, 0.2f);
            _camBody.Lookahead.Smoothing = 10f;
            _camBody.Lookahead.Time = 0.5f;
        }
    }
}
