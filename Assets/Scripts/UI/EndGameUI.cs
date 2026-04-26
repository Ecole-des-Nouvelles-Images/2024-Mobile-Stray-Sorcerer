using Manager;
using UnityEngine;

namespace UI
{
    public abstract class EndGameUI : MonoBehaviour
    {
        public void ReturnTitle()
        {
            Time.timeScale = 1;
            SceneLoader.Instance.LoadTitleScreen();
        }
    }
}
