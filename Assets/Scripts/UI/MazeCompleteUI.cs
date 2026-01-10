using Gameplay;
using Gameplay.GameData;
using Manager;
using TMPro;
using UnityEngine;

namespace UI
{
    public class MazeCompleteUI : EndGameUI
    {
        [Header("current run")]
        [SerializeField] private TMP_Text _timePassed;
        [SerializeField] private TMP_Text _monsterKillCount;
    
        [Header("best run")]
        [SerializeField] private TMP_Text _timePassedBRD;
        [SerializeField] private TMP_Text _monsterKillCountBRD;
        
        private int _hour;
        private int _minutes;
        private int _seconds;
        private TMP_Text _textToChange;
        
        private void SetupTimeDisplay(bool bestRunData)
        {
            if (bestRunData)
            {
                _textToChange = _timePassedBRD;
                _minutes = (int)(DataSaveSystem.Instance.BRDtime / 60);
                _seconds = Mathf.FloorToInt(DataSaveSystem.Instance.BRDtime % 60);
            }
            else
            {
                _textToChange = _timePassed;
                _minutes = (int)(ClockGame.Instance.TimerGame / 60);
                _seconds = Mathf.FloorToInt(ClockGame.Instance.TimerGame % 60);
            }
            if (_minutes >= 60) {
                _hour = _minutes / 60;
                _minutes -= 60 * _hour;
            }
            if (_hour == 0 && _minutes == 0)
                _textToChange.text = string.Format("{0,00}sec.", _seconds);
            else if(_hour == 0 && _minutes > 0)
                _textToChange.text = string.Format("{0,00:00}min. {1,1:00}sec.",_minutes,_seconds);
            else
                _textToChange.text = string.Format("{0,0:0}h. {1,1:00}min. {2,1:00}sec.",_hour,_minutes,_seconds);
        }
        private void SetupMazeCompleteStat()
        {
            _monsterKillCount.text = DataSaveSystem.Instance.Kill.ToString();
            _monsterKillCountBRD.text = DataSaveSystem.Instance.BRDkill.ToString();
        }
        /*public void ContinueGame()
        {
            GameManager.Instance.SaveDataAndContinue();
        }

        public void QuitGame()
        {
            GameManager.Instance.LeaveGame();
        }*/

        public void UpdateDisplay()
        {
            DataSaveSystem.Instance.RestoreBestRunData();
            SetupTimeDisplay(false);
            SetupTimeDisplay(true);
            SetupMazeCompleteStat();
        }
        
                
    }
}