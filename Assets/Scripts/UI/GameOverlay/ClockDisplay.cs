using Gameplay.GameData;
using TMPro;
using UnityEngine;

namespace UI.GameOverlay
{
    public class ClockDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text _timerTextDisplay;
        
        private void OnEnable()
        {
            ClockGame.Instance?.AddDisplayClock(_timerTextDisplay);
        }

        private void OnDisable()
        {
            ClockGame.Instance?.RemoveDisplayClock(_timerTextDisplay);
        }
    }
}
