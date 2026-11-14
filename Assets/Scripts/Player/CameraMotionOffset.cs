using Unity.Cinemachine;
using UnityEngine;

namespace Player
{
    public class CameraMotionOffset: MonoBehaviour
    {
        [SerializeField] private float _maximumForwardAmount;

        private CinemachineCamera _vCam;
        private CinemachinePositionComposer _composer;

        private Coroutine _cameraTrackingCoroutine;

        private void Start()
        {
            _vCam = GameObject.Find("Camera/VCam Player").GetComponent<CinemachineCamera>();
            _composer = _vCam.GetCinemachineComponent(CinemachineCore.Stage.Body) as CinemachinePositionComposer;

            _vCam.Follow = FindFirstObjectByType<Character>().transform;
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
            if (_composer)
                _composer.TargetOffset.z = Mathf.LerpUnclamped(0, _maximumForwardAmount, Mathf.Abs(input));
        }
    }
}
