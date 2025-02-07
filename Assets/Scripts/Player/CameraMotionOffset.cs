using Cinemachine;
using UnityEngine;

namespace Player
{
    public class CameraMotionOffset: MonoBehaviour
    {
        [SerializeField] private float _maximumForwardAmount;

        private CinemachineVirtualCamera _vCam;
        private CinemachineFramingTransposer _cameraFramingTransposer;

        private Coroutine _cameraTrackingCoroutine;

        private void Start()
        {
            _vCam = GameObject.Find("Camera/VCam Player").GetComponent<CinemachineVirtualCamera>();
            _cameraFramingTransposer = _vCam.GetCinemachineComponent<CinemachineFramingTransposer>();

            _vCam.Follow = FindObjectOfType<Character>().transform;
        }

        private void OnEnable()
        {
            PlayerController.OnPlayerMotion += OnMove;
        }

        private void OnDisable()
        {
            PlayerController.OnPlayerMotion -= OnMove;
        }

        private void OnMove(float input)
        {
            if (_cameraFramingTransposer)
                _cameraFramingTransposer.m_TrackedObjectOffset.z = Mathf.LerpUnclamped(0, _maximumForwardAmount, Mathf.Abs(input));
        }
    }
}
