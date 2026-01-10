using Gameplay.GameData;
using Manager;
using UnityEngine;

namespace UI
{
    public class MazeCompleteUI : EndGameUI
    {
        /*public void ContinueGame()
        {
            GameManager.Instance.SaveDataAndContinue();
        }

        public void QuitGame()
        {
            GameManager.Instance.LeaveGame();
        }*/

        public void ReturnTitle()
        {
            Time.timeScale = 1;
            DataSaveSystem.OnResetGameData?.Invoke();
            SceneLoader.Instance.LoadTitleScreen();
        }
    }
}