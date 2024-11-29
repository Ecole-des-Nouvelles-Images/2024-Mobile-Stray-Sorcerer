using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Inputs")]
        [SerializeField] private PlayerInput _playerInput;

        [Header("Settings")]
        [SerializeField] private float _accelerometerSensibility = 0.01f;
        [Tooltip("Internal modifier specific to the accelerometer")]
        [SerializeField] private float _accModifier = 1f;

        [Header("Animations")]
        [SerializeField] private Animator _animatorController;

        public static Action<bool> OnControlMapChanged;

        private Rigidbody _rb;
        private bool _isAccelerometerControlled = true;

        private void Awake() {
            _rb = GetComponent<Rigidbody>();
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
            Vector3 input = ctx.ReadValue<Vector3>();
            Vector3 direction = Vector3.zero;

            if (input.x > _accelerometerSensibility)
                direction.x = input.x;
            else if (input.x < -_accelerometerSensibility)
                direction.x = input.x;
            if (input.y > _accelerometerSensibility )
                direction.y = input.y;
            else if ( input.y < -_accelerometerSensibility)
                direction.y = input.y;

            _rb.velocity = new Vector3(direction.y * -1, 0, direction.x) * Character.Instance.Speed * _accModifier * Time.fixedDeltaTime;
            // TODO: Animations parameters / triggers
            // _animatorController.SetFloat(AnimatorParameterAccess.VelocityX, _rb.velocity.x);
            // _animatorController.SetFloat(AnimatorParameterAccess.VelocityY, _rb.velocity.z);
        }

        public void JoystickMove(InputAction.CallbackContext ctx)
        {
            Vector2 value = ctx.ReadValue<Vector2>();

            _rb.velocity = new Vector3(value.x, 0, value.y) * Character.Instance.Speed * Time.fixedDeltaTime;
            // TODO: Animations parameters / triggers
            // _animatorController.SetFloat(AnimatorParameterAccess.VelocityX,_rb.velocity.x);
            // _animatorController.SetFloat(AnimatorParameterAccess.VelocityY,_rb.velocity.z);
        }
    }
}