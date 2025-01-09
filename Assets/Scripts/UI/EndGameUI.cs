using Gameplay.GameData;
using TMPro;
using UnityEngine;

namespace UI
{
    public abstract class EndGameUI : MonoBehaviour
    {
        [Header("current run")]
        [SerializeField] private TMP_Text _mazeCompleteDisplay;
        [SerializeField] private TMP_Text _timePassed;
        [SerializeField] private TMP_Text _monsterKillCount;
        [SerializeField] private TMP_Text _playerLifeStat;
        [SerializeField] private TMP_Text _playerLevel;
        [SerializeField] private TMP_Text _playerConstitutionCount;
        [SerializeField] private TMP_Text _playerAttackSpeedCount;
        [SerializeField] private TMP_Text _playerPowerCount;
    
        [Header("best run")]
        [SerializeField] private TMP_Text _mazeCompleteDisplayBRD;
        [SerializeField] private TMP_Text _timePassedBRD;
        [SerializeField] private TMP_Text _monsterKillCountBRD;
        [SerializeField] private TMP_Text _playerLifeStatBRD;
        [SerializeField] private TMP_Text _playerLevelBRD;
        [SerializeField] private TMP_Text _playerConstitutionCountBRD;
        [SerializeField] private TMP_Text _playerAttackSpeedCountBRD;
        [SerializeField] private TMP_Text _playerPowerCountBRD;
    
        private int _hour;
        private int _minutes;
        private int _seconds;
        private TMP_Text _textToChange;

        private void SetupTimeDisplay(bool bestRunData)
        {
            if (bestRunData)
            {
                _textToChange = _timePassedBRD;
                _minutes = (int)(DataCollector.Instance.BRDtime / 60);
                _seconds = Mathf.FloorToInt(DataCollector.Instance.BRDtime % 60);
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
            _monsterKillCount.text = DataCollector.Instance.Kill.ToString();
            _mazeCompleteDisplay.text = DataCollector.Instance.MazeComplete.ToString();
            
            _monsterKillCountBRD.text = DataCollector.Instance.BRDkill.ToString();
            _mazeCompleteDisplayBRD.text = DataCollector.Instance.BRDmazeComplete.ToString();
        }

        private void SetupCharacterStat()
        {
            _playerLevel.text = DataCollector.Instance.PlayerLevel + "/20";
            _playerLifeStat.text = DataCollector.Instance.PlayerHp + "/" + DataCollector.Instance.PlayerMaxHp;
            _playerConstitutionCount.text = DataCollector.Instance.PlayerConstitution.ToString();
            _playerAttackSpeedCount.text = DataCollector.Instance.PlayerSwiftness.ToString();
            _playerPowerCount.text = DataCollector.Instance.PlayerPower.ToString();
            
            _playerLevelBRD.text = DataCollector.Instance.BRDplayerLevel + "/20";
            _playerLifeStatBRD.text = DataCollector.Instance.BRDplayerHp + "/" + DataCollector.Instance.BRDplayerMaxHp;
            _playerConstitutionCountBRD.text = DataCollector.Instance.BRDplayerConstitution.ToString();
            _playerAttackSpeedCountBRD.text = DataCollector.Instance.BRDplayerSwiftness.ToString();
            _playerPowerCountBRD.text = DataCollector.Instance.BRDplayerPower.ToString();
        }
    
        public void UpdateDisplay()
        {
            DataCollector.Instance.RestoreBestRunData();
            SetupTimeDisplay(false);
            SetupTimeDisplay(true);
            SetupMazeCompleteStat();
            SetupCharacterStat();
        }
        
    }
}
