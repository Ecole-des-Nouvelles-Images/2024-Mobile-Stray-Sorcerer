using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

namespace Player
{
    public class CameraMotionOffset: MonoBehaviour
    {
        [SerializeField] private float _maximumForwardAmount;
        [SerializeField] private float _cameraTrackingReactivity = 0.5f;

        private CinemachineFramingTransposer _cameraFramingTransposer;
        private float _currentForwardAmount;

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

            _currentForwardAmount = input;
        }

        [Obsolete("This method is deprecated for poor camera handling", true)]
        private void OnMoveOld(float input)
        {
            if (_currentForwardAmount < 0.4f && input >= 0.4f)
            {
                if (_cameraTrackingCoroutine != null)
                    StopAllCoroutines();
                _cameraTrackingCoroutine = StartCoroutine(SmoothCameraTrackingOffset(1));
            }
            else if (_currentForwardAmount > -0.4f && input <= -0.4f)
            {
                if (_cameraTrackingCoroutine != null)
                    StopAllCoroutines();
                _cameraTrackingCoroutine = StartCoroutine(SmoothCameraTrackingOffset(1));
            }
            else if ((_currentForwardAmount < -0.4f && input is >= -0.4f and <= 0.4f)
                     || (_currentForwardAmount > 0.4f && input is >= -0.4f and <= 0.4f))
            {
                if (_cameraTrackingCoroutine != null)
                    StopAllCoroutines();
                _cameraTrackingCoroutine = StartCoroutine(SmoothCameraTrackingOffset(0));
            }
        }

        [Obsolete("Used inside deprecated method OnMoveOld()", true)]
        private IEnumerator SmoothCameraTrackingOffset(int direction)
        {
            float t = 0f;
            float startValue = _cameraFramingTransposer.m_TrackedObjectOffset.z;

            while (t < 1)
            {
                t += Time.deltaTime / _cameraTrackingReactivity;
                _cameraFramingTransposer.m_TrackedObjectOffset.z = Mathf.Lerp(startValue, _maximumForwardAmount * direction, t);
                yield return null;
            }
        }
    }
}
