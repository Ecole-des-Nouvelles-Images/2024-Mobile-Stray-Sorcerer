using Gameplay;
using Gameplay.GameData;
using Player;
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
        [SerializeField] private TMP_Text _playerLevel;
        [SerializeField] private TMP_Text _playerConstitutionCount;
        [SerializeField] private TMP_Text _playerAttackSpeedCount;
        [SerializeField] private TMP_Text _playerPowerCount;
    
        [Header("best run")]
        [SerializeField] private TMP_Text _mazeCompleteDisplayBRD;
        [SerializeField] private TMP_Text _timePassedBRD;
        [SerializeField] private TMP_Text _monsterKillCountBRD;
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
            _mazeCompleteDisplay.text = DataSaveSystem.Instance.MazeComplete.ToString();
            _monsterKillCountBRD.text = DataSaveSystem.Instance.BRDkill.ToString();
            _mazeCompleteDisplayBRD.text = DataSaveSystem.Instance.BRDmazeComplete.ToString();
        }

        private void SetupCharacterStat()
        {
            _playerLevel.text = DataSaveSystem.Instance.GetPlayerLevelData() + "/20";
            _playerConstitutionCount.text = DataSaveSystem.Instance.GetPlayerConstitutionData().ToString();
            _playerAttackSpeedCount.text = DataSaveSystem.Instance.GetPlayerSwiftnessData().ToString();
            _playerPowerCount.text = DataSaveSystem.Instance.GetPlayerPowerData().ToString();
            
            _playerLevelBRD.text = DataSaveSystem.Instance.BRDplayerLevel + "/20";
            _playerConstitutionCountBRD.text = DataSaveSystem.Instance.BRDplayerConstitution.ToString();
            _playerAttackSpeedCountBRD.text = DataSaveSystem.Instance.BRDplayerSwiftness.ToString();
            _playerPowerCountBRD.text = DataSaveSystem.Instance.BRDplayerPower.ToString();
        }
    
        public void UpdateDisplay()
        {
            DataSaveSystem.Instance.RestoreBestRunData();
            SetupTimeDisplay(false);
            SetupTimeDisplay(true);
            SetupMazeCompleteStat();
            SetupCharacterStat();
        }
        
    }
}
