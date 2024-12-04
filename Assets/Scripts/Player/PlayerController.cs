using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Inputs")]
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private float _accelerometerSensibility = 0.01f;
        [Tooltip("Internal modifier specific to the accelerometer")]
        [SerializeField] private float _accModifier = 1f;

        [Header("Animations")]
        [SerializeField] private Animator _animatorController;

        [Header("Settings")]
        [SerializeField] private float _cameraTransposerMaxOffset;
        [SerializeField] private float _cameraTrackingReactivity = 0.5f;

        public static Action<bool> OnControlMapChanged;

        public bool IsStandby { get; set; } = false;

        private CinemachineFramingTransposer _cameraFramingTransposer;
        private Rigidbody _rb;
        private Coroutine _cameraTrackingCoroutine;

        private float _currentForwardAmount;
        private bool _isAccelerometerControlled = true;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _cameraFramingTransposer = GameObject.FindWithTag("MainCamera").GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
        }

        public void SwitchController()
        {
            if (_isAccelerometerControlled)
            {
                _playerInput.SwitchCurrentActionMap("Tactile");
                _isAccelerometerControlled = false;
                OnControlMapChanged.Invoke(true);
            }
            else
            {
                _playerInput.SwitchCurrentActionMap("Accelerometer");
                _isAccelerometerControlled = true;
                OnControlMapChanged.Invoke(false);
            }
        }

        public void AccelerometerMove(InputAction.CallbackContext ctx)
        {
            if (IsStandby) return;

            Vector3 input = ctx.ReadValue<Vector3>();
            Vector3 direction = Vector3.zero;

            if (input.x > _accelerometerSensibility)
                direction.x = input.x;
            else if (input.x < -_accelerometerSensibility)
                direction.x = input.x;
            if (input.y > _accelerometerSensibility)
                direction.y = input.y;
            else if (input.y < -_accelerometerSensibility)
                direction.y = input.y;

            _rb.velocity = new Vector3(direction.y * -1, 0, direction.x) * Character.Instance.Speed * _accModifier * Time.fixedDeltaTime;
            // TODO: Animations parameters / triggers
            // _animatorController.SetFloat(AnimatorParameterAccess.VelocityX, _rb.velocity.x);
            // _animatorController.SetFloat(AnimatorParameterAccess.VelocityY, _rb.velocity.z);
        }

        public void JoystickMove(InputAction.CallbackContext ctx)
        {
            if (IsStandby) return;

            Vector2 value = ctx.ReadValue<Vector2>();

            _rb.velocity = new Vector3(value.x, 0, value.y) * Character.Instance.Speed * Time.fixedDeltaTime;

            if (_currentForwardAmount < 0.4f && value.y >= 0.4f)
            {
                if (_cameraTrackingCoroutine != null)
                    StopAllCoroutines();
                _cameraTrackingCoroutine = StartCoroutine(SmoothCameraTrackingOffset(1));
            }
            else if (_currentForwardAmount > -0.4f && value.y <= -0.4f)
            {
                if (_cameraTrackingCoroutine != null)
                    StopAllCoroutines();
                _cameraTrackingCoroutine = StartCoroutine(SmoothCameraTrackingOffset(-1));
            }
            else if ((_currentForwardAmount < -0.4f && (value.y is >= -0.4f and <= 0.4f))
                    || (_currentForwardAmount > 0.4f && (value.y is >= -0.4f and <= 0.4f)))
            {
                if (_cameraTrackingCoroutine != null)
                    StopAllCoroutines();
                _cameraTrackingCoroutine = StartCoroutine(SmoothCameraTrackingOffset(0));
            }

            _currentForwardAmount = value.y;
        }

        // TODO: Animations parameters / triggers
        // _animatorController.SetFloat(AnimatorParameterAccess.VelocityX,_rb.velocity.x);
        // _animatorController.SetFloat(AnimatorParameterAccess.VelocityY,_rb.velocity.z);

        private IEnumerator SmoothCameraTrackingOffset(int direction)
        {
            float t = 0f;
            float startValue = _cameraFramingTransposer.m_TrackedObjectOffset.z;

            while (t < 1)
            {
                t += Time.deltaTime / _cameraTrackingReactivity;
                _cameraFramingTransposer.m_TrackedObjectOffset.z = Mathf.Lerp(startValue, _cameraTransposerMaxOffset * direction, t);
                yield return null;
            }
        }
    }
}