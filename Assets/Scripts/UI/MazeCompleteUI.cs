using Gameplay;
using Gameplay.GameData;
using Manager;
using Player;
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
                _textToChange.text = string.Format("{0,00}sec.", _seconds);
            else if(_hour == 0 && _minutes > 0)
                _textToChange.text = string.Format("{0,00:00}min. {1,1:00}sec.",_minutes,_seconds);
            else
                _textToChange.text = string.Format("{0,0:0}h. {1,1:00}min. {2,1:00}sec.",_hour,_minutes,_seconds);
        }
        private void SetupMazeCompleteStat()
        {
            _monsterKillCount.text = DataSaveSystem.Instance.Kill.ToString();
            _monsterKillCountBRD.text = DataSaveSystem.Instance.BRDKill.ToString();
        }

        public void UpdateDisplay()
        {
            SetupMazeCompleteStat();
            SetupTimeDisplay(_timePassed);
            SetupTimeDisplay(_timePassedBRD);
            if(Character.Instance.gameObject)Destroy(Character.Instance.gameObject);
        }
        
                
    }
}