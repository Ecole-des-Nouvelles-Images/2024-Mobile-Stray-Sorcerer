using UnityEngine;

namespace Utils
{
    public class SwitchControl : MonoBehaviour
    {
        public bool IsAccelerometer;
        [SerializeField] private GameObject _joystick;

        private void Awake()
        {
            IsAccelerometer = true;
            _joystick.SetActive(false);
        }

        public void SwitchControlOfPlayer()
        {
            if (IsAccelerometer)
            {
                IsAccelerometer = false;
                _joystick.SetActive(true);
                Debug.Log(IsAccelerometer);
            }
            else
            {
                IsAccelerometer = true;
                _joystick.SetActive(false);
                Debug.Log(IsAccelerometer);
            }
        }
    }
}