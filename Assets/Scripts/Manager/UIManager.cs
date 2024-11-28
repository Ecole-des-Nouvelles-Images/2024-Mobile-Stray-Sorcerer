using UnityEngine;
using Utils;

namespace Manager
{
    public class UIManager : SingletonMonoBehaviour<UIManager>
    {
        [SerializeField] private GameObject _pauseOverlay;

        public void SetPause(bool enable)
        {
            Time.timeScale = enable ? 0 : 1;
            _pauseOverlay.SetActive(enable);
        }
    }
}
