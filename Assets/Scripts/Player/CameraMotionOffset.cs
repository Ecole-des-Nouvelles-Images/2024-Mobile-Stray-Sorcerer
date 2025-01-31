using Cinemachine;
using UnityEngine;

namespace Player
{
    public class CameraMotionOffset: MonoBehaviour
    {
        [SerializeField] private float _maximumForwardAmount;

        private CinemachineFramingTransposer _cameraFramingTransposer;

        private Coroutine _cameraTrackingCoroutine;

        private void Awake()
        {
            _cameraFramingTransposer = GameObject.Find("VCam Player").GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();

            Debug.Log($"Camera status : {(_cameraFramingTransposer == null ? "null" : "valid")}");
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
            _cameraFramingTransposer.m_TrackedObjectOffset.z = Mathf.Lerp(-_maximumForwardAmount, _maximumForwardAmount, input);
        }
    }
}
