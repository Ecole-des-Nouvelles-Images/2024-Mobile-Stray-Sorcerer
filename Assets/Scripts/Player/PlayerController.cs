using System;
using Manager;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Utils;
using Vector2 = UnityEngine.Vector2;

namespace Player
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
        public static readonly int IsMoving = Animator.StringToHash("isMoving");

        [Header("Reference")]
        [SerializeField] private Animator _characterAnimator;

        [Header("Settings")]
        [Tooltip("Delay in seconds of the duration of the offset transition")]
        [SerializeField] private float _cameraTransposerMaxOffset;

        public static Action<bool> OnControlMapChanged;
        public static Action<float> OnPlayerMotion;

        public bool IsStandby { get; set; } = false;

        private CinemachinePositionComposer _cameraFramingTransposer;
        private Rigidbody _rb;
        
        public static PlayerInput PlayerInput { get; private set; }
        private InputActionMap _playerActions;
        private InputActionMap _uiActions;
        
        private Coroutine _cameraTrackingCoroutine;

        private float _currentForwardAmount;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void OnRuntimeInitialization()
        {
            OnPlayerMotion = null;
            OnControlMapChanged = null;
        }

        private void Start()
        {            
            _rb = GetComponent<Rigidbody>();

            PlayerInput = GetComponent<PlayerInput>();
            _playerActions = PlayerInput.actions.FindActionMap("Player");
            _uiActions = PlayerInput.actions.FindActionMap("UI");
            
            ManageCallbacks(CallbackOperation.Subscribe);
        }

        private void OnDestroy()
        {
            ManageCallbacks(CallbackOperation.Unsubscribe);
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

        private void ManageCallbacks(CallbackOperation operation)
        {
            if (operation == CallbackOperation.Unsubscribe)
            {
                if (UIManager.Instance)
                    UIManager.Instance.OnTogglePauseState -= SwitchToUIActionMap;
                
                _playerActions.FindAction("Move").performed -= OnMove;
                _playerActions.FindAction("Pause").started -= OnPause;
                
                _uiActions.FindAction("Resume").started -= OnResume;
                _uiActions.FindAction("Navigate").started -= OnNavigate;
                _uiActions.FindAction("Submit").started -= OnSubmit;
                _uiActions.FindAction("Cancel").started -= OnCancel;
            }
            else
            {
                UIManager.Instance.OnTogglePauseState += SwitchToUIActionMap;
                
                _playerActions.FindAction("Move").performed += OnMove;
                _playerActions.FindAction("Pause").started += OnPause;
                
                _uiActions.FindAction("Resume").started += OnResume;
                _uiActions.FindAction("Navigate").started += OnNavigate;
                _uiActions.FindAction("Submit").started += OnSubmit;
                _uiActions.FindAction("Cancel").started += OnCancel;
            }
        }

        public void SwitchToUIActionMap(bool enable)
        {
            PlayerInput.SwitchCurrentActionMap(enable ? "UI" : "Player");
        }

        #region PlayerInputActions callbacks
        
        public void OnMove(InputAction.CallbackContext context)
        {
            if (IsStandby) return;

            Vector2 value = context.ReadValue<Vector2>();

            _rb.linearVelocity = new Vector3(value.x, 0, value.y) * Character.Instance.Speed * Time.fixedDeltaTime;

            OnPlayerMotion?.Invoke(value.y);
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            UIManager.Instance.SwitchPausePanel();
        }

        public void OnResume(InputAction.CallbackContext context)
        {
            UIManager.Instance.SwitchPausePanel();
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
        
        #region Experimental
        
        // [Header("Inputs")]
        // [SerializeField] private float _accelerometerSensibility = 0.01f;
        // [Tooltip("Internal modifier specific to the accelerometer")]
        // [SerializeField] private float _accModifier = 1f;
        
        // private bool _isAccelerometerControlled = true;
        
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
        
        #endregion
    }
}
