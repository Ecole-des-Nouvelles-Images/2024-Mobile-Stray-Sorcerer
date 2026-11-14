using Gameplay.GameSaveDataSystem;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UI;

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

        [Header("Button")]
        [SerializeField] private Button _returnToTitleScreen;
        
        private int _hour;
        private int _minutes;
        private int _seconds;
        private TMP_Text _textToChange;
        
        private void SetupTimeDisplay(TMP_Text timeToFormate)
        {
            _textToChange = timeToFormate;
            _minutes = (int)(DataSaveSystem.Instance.BRDTime / 60);
            _seconds = Mathf.FloorToInt(DataSaveSystem.Instance.BRDTime % 60);
            if (_minutes >= 60) {
                _hour = _minutes / 60;
                _minutes -= 60 * _hour;
            }
            if (_hour == 0 && _minutes == 0)
                LocalizationText.Set(_textToChange, "TIME_SECONDS", _seconds);
            else if(_hour == 0 && _minutes > 0)
                LocalizationText.Set(_textToChange, "TIME_MINUTES_SECONDS", _minutes, _seconds);
            else
                LocalizationText.Set(_textToChange, "TIME_HOURS_MINUTES_SECONDS", _hour, _minutes, _seconds);
        }
        private void SetupMazeCompleteStat()
        {
            _monsterKillCount.text = DataSaveSystem.Instance.Kill.ToString();
            _monsterKillCountBRD.text = DataSaveSystem.Instance.BRDKill.ToString();
        }

        public void UpdateDisplay()
        {
            _returnToTitleScreen.Select();
            
            SetupMazeCompleteStat();
            SetupTimeDisplay(_timePassed);
            SetupTimeDisplay(_timePassedBRD);
            if(Character.Instance.gameObject)Destroy(Character.Instance.gameObject);
        }
        
                
    }
}
