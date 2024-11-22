using UnityEngine;

namespace Utils
{
    public class SwitchControl : MonoBehaviour
    {
        public bool IsAccelerometer;
        [SerializeField]private GameObject joystick;
        private void Awake() {
            IsAccelerometer = true;
            joystick.SetActive(false);
        }

        public void SwitchControlOfPlayer() {
            if (IsAccelerometer) {
                IsAccelerometer = false;
                joystick.SetActive(true);
                Debug.Log(IsAccelerometer);
            }
            else {
                IsAccelerometer = true;
                joystick.SetActive(false);
                Debug.Log(IsAccelerometer);
            }
        }
    }
}
