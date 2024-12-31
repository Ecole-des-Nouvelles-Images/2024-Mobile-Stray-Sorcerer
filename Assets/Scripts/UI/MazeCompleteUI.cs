using Gameplay.GameData;

namespace UI
{
    public class MazeCompleteUI : EndGameUI
    {
        public void ContinueGame()
        {
            DataCollector.Instance.SaveDataAndContinue();
        }

        public void QuitGame()
        {
            DataCollector.Instance.SaveDataAndLeave();
        }
    }
}