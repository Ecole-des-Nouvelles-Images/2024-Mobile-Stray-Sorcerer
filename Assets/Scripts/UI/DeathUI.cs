using Gameplay.GameData;
using UnityEngine.SceneManagement;

namespace UI
{
    public class DeathUI : EndGameUI
    {
        public void ReturnTitle()
        {
            SceneManager.LoadScene("Setup");
        }
    }
}