using Gameplay.GameData;
using Manager;

namespace UI
{
    public class MazeCompleteUI : EndGameUI
    {
        public void ContinueGame()
        {
            GameManager.Instance.SaveDataAndContinue();
        }

        public void QuitGame()
        {
            GameManager.Instance.LeaveGame();
        }
    }
}