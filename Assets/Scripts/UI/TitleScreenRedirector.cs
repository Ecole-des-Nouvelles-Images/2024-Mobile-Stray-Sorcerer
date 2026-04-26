using Manager;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class TitleScreenRedirector : MonoBehaviour
    {
        [SerializeField] private InputAction _startAction;

        private void Awake()
        {
            _startAction.Enable();
        }

        private void OnEnable()
        {
            _startAction.started += OnStartPerformed;
        }
        
        private void OnDisable()
        {
            _startAction.started -= OnStartPerformed;
            _startAction.Disable();
        }
        
        private void OnStartPerformed(InputAction.CallbackContext context)
        {
            SceneLoader.Instance.LaunchGame();
        }
    }
}
