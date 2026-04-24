using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Manager
{
    public class TitleScreenRedirector : MonoBehaviour
    {
        [SerializeField] private List<InputActionReference> _startActions;
        
        private void OnEnable()
        {
            foreach (var input in _startActions)
            {
                input.action.performed += OnStartPerformed;
            }
        }
        
        private void OnDisable()
        {
            foreach (var input in _startActions)
            {
                input.action.performed -= OnStartPerformed;
            }
        }
        
        private void OnStartPerformed(InputAction.CallbackContext context)
        {
            SceneLoader.Instance.LaunchGame();
        }
    }
}
