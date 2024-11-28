using Player;
using UnityEngine;
using Utils;

namespace Manager
{
    public class UIManager : SingletonMonoBehaviour<UIManager>
    {
        [Header("UI Panels")]
        [SerializeField] private GameObject _pauseOverlay;

        [Header("References")]
        [SerializeField] private GameObject _joystick;

        private void OnEnable()
        {
            PlayerController.OnControlMapChanged += ActivateJoystick;
        }

        private void OnDisable()
        {
            PlayerController.OnControlMapChanged -= ActivateJoystick;
        }

        public void SetPause(bool enable)
        {
            Time.timeScale = enable ? 0 : 1;
            _pauseOverlay.SetActive(enable);
        }

        public void SwitchControlMap()
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().SwitchController();
        }

        private void ActivateJoystick(bool state)
        {
            _joystick.SetActive(state);
        }
    }
}
