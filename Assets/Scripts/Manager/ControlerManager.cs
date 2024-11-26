using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Manager {
    public class ControlerManager : SingletonMonoBehaviour<ControlerManager> {
        public bool isAccelerometerControled {get; private set;}
        
        [SerializeField]private PlayerInput playerInput;
        [SerializeField]private GameObject TactileJoysticks;

        private void Awake() {
            playerInput.SwitchCurrentActionMap("Accelerometer");
            TactileJoysticks.SetActive(false);
            isAccelerometerControled = true;
        }

        public void SwitchControler() {
            if (isAccelerometerControled) {
                playerInput.SwitchCurrentActionMap("Tactile");
                TactileJoysticks.SetActive(true);
                isAccelerometerControled = false;
            }
            else {
                playerInput.SwitchCurrentActionMap("Accelerometer");
                TactileJoysticks.SetActive(false);
                isAccelerometerControled = true;
            }
        }
        public void SetMenuMap() {
            playerInput.SwitchCurrentActionMap("UI");
            if(TactileJoysticks.activeSelf)TactileJoysticks.SetActive(false);
        }
        public void SetGameMap() {
            switch (isAccelerometerControled) {
                case true:
                    playerInput.SwitchCurrentActionMap("Accelerometer");
                    TactileJoysticks.SetActive(false);
                    isAccelerometerControled = true;
                    return;
                case false:
                    playerInput.SwitchCurrentActionMap("Tactile");
                    TactileJoysticks.SetActive(true);
                    isAccelerometerControled = false;
                    return;
            }
        }
    }
}
