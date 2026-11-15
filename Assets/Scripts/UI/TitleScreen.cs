using Manager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI
{
    public class TitleScreen : MonoBehaviour
    {
        [SerializeField] private float _timerBeforeInteractable = 3f;
        [Header("Gamepad redirects")]
        [SerializeField] private InputAction _startAction;
        [SerializeField] private InputAction _quitAction;
        [SerializeField] private Image _holdGauge;

        private Button _touchToPlay;
        private bool _isQuitting;
        private float _quitTimer;
        private const float QuitHoldDuration = 3f;

        private void Awake()
        {
            _startAction.Enable();

            if (_quitAction.bindings.Count == 0)
            {
                _quitAction.AddBinding("<Gamepad>/select").WithInteraction("hold(duration=3)");
            }
            _quitAction.Enable();
        }

        private void OnEnable()
        {
            _touchToPlay = GetComponent<Button>();
            _touchToPlay.onClick.AddListener(StartGame);
            _touchToPlay.onClick.AddListener(DisableButton);

            _startAction.started += OnStartPerformed;
            _quitAction.started += OnQuitStarted;
            _quitAction.canceled += OnQuitCanceled;
            _quitAction.performed += OnQuitPerformed;
        }

        private void OnDisable()
        {
            _touchToPlay.onClick.RemoveAllListeners();

            _startAction.started -= OnStartPerformed;
            _startAction.Disable();

            _quitAction.started -= OnQuitStarted;
            _quitAction.canceled -= OnQuitCanceled;
            _quitAction.performed -= OnQuitPerformed;
            _quitAction.Disable();
        }

        private void Update()
        {
            if (_timerBeforeInteractable > 0)
                _timerBeforeInteractable -= Time.deltaTime;
            else
                _touchToPlay.interactable = true;

            if (_isQuitting)
            {
                _quitTimer += Time.deltaTime;
                if (_holdGauge != null)
                {
                    _holdGauge.fillAmount = Mathf.Clamp01(_quitTimer / QuitHoldDuration);
                }
            }
        }

        private void OnStartPerformed(InputAction.CallbackContext context)
        {
            StartGame();
        }

        private void OnQuitStarted(InputAction.CallbackContext context)
        {
            _isQuitting = true;
            _quitTimer = 0f;
        }

        private void OnQuitCanceled(InputAction.CallbackContext context)
        {
            ResetQuitGauge();
        }

        private void OnQuitPerformed(InputAction.CallbackContext context)
        {
            ResetQuitGauge();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void ResetQuitGauge()
        {
            _isQuitting = false;
            _quitTimer = 0f;
            if (_holdGauge != null)
            {
                _holdGauge.fillAmount = 0f;
            }
        }

        private void StartGame()
        {
            SceneLoader.Instance.LaunchGame();
            _startAction.Disable();
        }

        private void DisableButton()
        {
            _touchToPlay.onClick.RemoveAllListeners();
            _touchToPlay.interactable = false;
        }
    }
}
