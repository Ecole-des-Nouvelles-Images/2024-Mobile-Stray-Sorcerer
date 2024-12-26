using Gameplay.GameData;
using TMPro;
using UnityEngine;
using Utils;

namespace UI.GameOverlay
{
    public class ClockDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text _timerTextDisplay;

        private void Awake()
        {
            ClockGame.Instance.SetDisplayClock(_timerTextDisplay);
        }
    }
}
