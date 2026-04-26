using System;
using Manager;
using Scripts.Player;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

using PlayerInputActions = Scripts.Player.PlayerInputActions;
using Vector2 = UnityEngine.Vector2;

namespace Player
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour, PlayerInputActions.IPlayerActions, PlayerInputActions.IUIActions 
    {
        public static readonly int IsMoving = Animator.StringToHash("isMoving");

        [Header("Reference")]
        [SerializeField] private Animator _characterAnimator;

        [Header("Inputs")]
        [SerializeField] private float _accelerometerSensibility = 0.01f;
        [Tooltip("Internal modifier specific to the accelerometer")]
        [SerializeField] private float _accModifier = 1f;

        [Header("Settings")]
        [Tooltip("Delay in seconds of the duration of the offset transition")]
        [SerializeField] private float _cameraTransposerMaxOffset;

        public static Action<bool> OnControlMapChanged;
        public static Action<float> OnPlayerMotion;

        public bool IsStandby { get; set; } = false;

        private CinemachinePositionComposer _cameraFramingTransposer;
        private Rigidbody _rb;
        
        private PlayerInputActions _playerInputActions;
        private PlayerInput _playerInput;
        private EventSystem _eventSystem;
        
        private Coroutine _cameraTrackingCoroutine;

        private float _currentForwardAmount;
        // private bool _isAccelerometerControlled = true;

        private void Start()
        {
            _playerInputActions = new PlayerInputActions();
            _playerInput = GetComponent<PlayerInput>();
            _eventSystem = FindFirstObjectByType<EventSystem>();
            
            _rb = GetComponent<Rigidbody>();
            
            // Tell the static InputSystem to defaults to the "Player" action map.
            InputSystem.actions.Disable();
            InputSystem.actions.FindActionMap("Player").Enable();
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

        #region PlayerInputActions callbacks
        
        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.canceled || context.started) return;
            
            if (IsStandby) return;

            Vector2 value = context.ReadValue<Vector2>();

            _rb.linearVelocity = new Vector3(value.x, 0, value.y) * Character.Instance.Speed * Time.fixedDeltaTime;

            OnPlayerMotion?.Invoke(value.y);
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.canceled || context.started) return;
            
            _playerInput.SwitchCurrentActionMap("UI");
            
            UIManager.Instance.SwitchPausePanel();
            
            Debug.Log($"Current action map : {InputSystem.actions.name}");
        }

        public void OnResume(InputAction.CallbackContext context)
        {
            if (context.canceled || context.started) return;
            
            _playerInput.SwitchCurrentActionMap("Player");
            
            UIManager.Instance.SwitchPausePanel();
            
            Debug.Log($"Current action map : {InputSystem.actions.name}");
        }

        public void OnNavigate(InputAction.CallbackContext context)
        {
            AxisEventData data = new (EventSystem.current)
            {
                moveDir = context.ReadValue<Vector2>() switch
                {
                    { y: > 0.5f } => MoveDirection.Up,
                    { y: < -0.5f } => MoveDirection.Down,
                    { x: > 0.5f } => MoveDirection.Right,
                    { x: < -0.5f } => MoveDirection.Left,
                    _ => MoveDirection.None
                },
                selectedObject = EventSystem.current.currentSelectedGameObject
            };

            ExecuteEvents.Execute(data.selectedObject, data, ExecuteEvents.moveHandler);
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
            // Not used manually, used by UI Input Module
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            // Not used manually, used by UI Input Module
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
            // Not used manually, used by UI Input Module
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            // Not used manually, used by UI Input Module
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
            // Not used manually, used by UI Input Module
        }

        public void OnMiddleClick(InputAction.CallbackContext context)
        {
            // Not used manually, used by UI Input Module
        }

        public void OnScrollWheel(InputAction.CallbackContext context)
        {
            // Not used manually, used by UI Input Module
        }

        public void OnTrackedDevicePosition(InputAction.CallbackContext context)
        {
            // Not used manually, used by UI Input Module
        }

        public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
        {
            // Not used manually, used by UI Input Module
        }
        
        #endregion
    }
}
