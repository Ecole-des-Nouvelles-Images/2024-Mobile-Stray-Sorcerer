using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using Cinemachine;

using Utils;

namespace Manager
{
    public class GameManager: SingletonMonoBehaviour<GameManager>
    {
        [Header("References")]
        [SerializeField] private GameObject _interfaceOverlay;
        [SerializeField] private GameObject _playerPrefab;

        [Header("Settings")]
        [SerializeField] private Vector3 _playerSpawnPosition;
        [SerializeField] private Vector3 _cameraOrientation;
        [SerializeField] private float _cameraFOV = 60;
        [SerializeField] private float _cameraDistance = 25;

        private GameObject _player;
        private CinemachineVirtualCamera _camera;

        private void Start()
        {
            _camera = GameObject.Find("VCam Player").GetComponent<CinemachineVirtualCamera>();
            _camera.transform.rotation = Quaternion.identity;
        }

        public void StartGame()
        {
            _camera.gameObject.SetActive(true);
            _interfaceOverlay.SetActive(true);

            _player = Instantiate(_playerPrefab, _playerSpawnPosition, Quaternion.identity);
            _player.GetComponent<PlayerInput>().uiInputModule = FindObjectOfType<InputSystemUIInputModule>();

            SetupCamera();
        }

        private void SetupCamera()
        {
            CinemachineFramingTransposer camBody = _camera.AddCinemachineComponent<CinemachineFramingTransposer>();

            _camera.transform.Rotate(_cameraOrientation);
            _camera.m_Follow = _player.transform;
            _camera.m_Lens.FieldOfView = _cameraFOV;
            camBody.m_CameraDistance = _cameraDistance;
            camBody.m_SoftZoneWidth = 0.2f;
            camBody.m_SoftZoneHeight = 0.2f;
        }
    }
}