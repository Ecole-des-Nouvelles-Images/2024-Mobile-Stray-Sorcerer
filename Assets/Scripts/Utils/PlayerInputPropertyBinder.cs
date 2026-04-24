using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace Utils
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInputPropertyBinder: MonoBehaviour
    {
        private PlayerInput _playerInputComponent;

        private void Awake()
        {
            _playerInputComponent = GetComponent<PlayerInput>();

            if (!(_playerInputComponent.uiInputModule = FindFirstObjectByType<InputSystemUIInputModule>()))
                Debug.LogWarning("[PlayerInput] No InputSystemUIInputModule found in the scene, UI input will not work");
            
            if (!(_playerInputComponent.camera = GameObject.Find("Camera/CinemachineBrain").GetComponent<Camera>()))
                Debug.LogWarning("[PlayerInput] No main camera found in the scene, UI input will not work");
        }
    }
}
