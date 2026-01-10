using Gameplay;
using Gameplay.GameData;
using Manager;
using Player;
using TMPro;
using UnityEngine;

namespace UI
{
    public abstract class EndGameUI : MonoBehaviour
    {
        public void ReturnTitle()
        {
            Time.timeScale = 1;
            DataSaveSystem.OnResetGameData?.Invoke();
            SceneLoader.Instance.LoadTitleScreen();
        }
    }
}
