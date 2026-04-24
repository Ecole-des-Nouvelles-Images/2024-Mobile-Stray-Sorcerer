using System;
using Manager;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public static readonly int IsMoving = Animator.StringToHash("isMoving");

        [Header("Reference")]
        [SerializeField] private Animator _characterAnimator;

        [Header("Inputs")]
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private float _accelerometerSensibility = 0.01f;

        [Tooltip("Internal modifier specific to the accelerometer")]
        [SerializeField] private float _accModifier = 1f;

        [Header("Settings")]
        [SerializeField] private float _cameraTransposerMaxOffset;
        [Tooltip("Delay in seconds of the duration of the offset transition")]

        public static Action<bool> OnControlMapChanged;
        public static Action<float> OnPlayerMotion;

        public bool IsStandby { get; set; } = false;

        private CinemachinePositionComposer _cameraFramingTransposer;
        private Rigidbody _rb;
        private Coroutine _cameraTrackingCoroutine;

        private float _currentForwardAmount;
        // private bool _isAccelerometerControlled = true;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            
            // Default the player to the "Game" action map.
            InputSystem.actions.Disable();
            InputSystem.actions.FindActionMap("Player").Enable();
        }

        private void OnEnable()
        {
            InputSystem.actions.FindAction("Pause").performed += OnPauseActionMapSwitch;
            InputSystem.actions.FindAction("Pause").performed += UIManager.Instance.SwitchPausePanel;
            InputSystem.actions.FindAction("Move").performed += JoystickMove;

        }

        private void OnDisable()
        {
            InputSystem.actions.FindAction("Pause").performed -= OnPauseActionMapSwitch;
            InputSystem.actions.FindAction("Pause").performed -= UIManager.Instance.SwitchPausePanel;
            InputSystem.actions.FindAction("Move").performed -= JoystickMove;

        }

        private void Update()
        {
            _characterAnimator.SetBool(IsMoving, _rb.linearVelocity != Vector3.zero);
            if (_rb.linearVelocity != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(_rb.linearVelocity, Vector3.up);
                _rb.MoveRotation(targetRotation);
            }
        }
        
        public void OnPauseActionMapSwitch(InputAction.CallbackContext ctx)
        {
            if (UIManager.Instance.InPause)
            {
                InputSystem.actions.FindActionMap("UI").Disable();
                InputSystem.actions.FindActionMap("Player").Enable();
            }
            else
            {
                InputSystem.actions.FindActionMap("UI").Enable();
                InputSystem.actions.FindActionMap("Player").Disable();
            }

        }

        /*public void SwitchController()
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
        }*/

        /*public void AccelerometerMove(InputAction.CallbackContext ctx)
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

            _rb.linearVelocity = new Vector3(direction.y * -1, 0, direction.x) * Character.Instance.Speed * _accModifier * Time.fixedDeltaTime;
        }*/

        public void JoystickMove(InputAction.CallbackContext ctx)
        {
            if (IsStandby) return;

            Vector2 value = ctx.ReadValue<Vector2>();

            _rb.linearVelocity = new Vector3(value.x, 0, value.y) * Character.Instance.Speed * Time.fixedDeltaTime;

            OnPlayerMotion?.Invoke(value.y);
        }
    }
}
