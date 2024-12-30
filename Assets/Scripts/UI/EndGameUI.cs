using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay.GameData;
using TMPro;
using UnityEngine;

public class EndGameUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _mazeCompleteDisplay;
    [SerializeField] private TMP_Text _timePassed;
    [SerializeField] private TMP_Text _monsterKillCount;
    [SerializeField] private TMP_Text _playerLifeStat;
    [SerializeField] private TMP_Text _playerLevel;
    [SerializeField] private TMP_Text _playerConstitutionCount;
    [SerializeField] private TMP_Text _playerAttackSpeedCount;
    [SerializeField] private TMP_Text _playerPowerCount;

    private int _hour;
    private int _minutes;
    private int _seconds;

    

    private void SetupTimeDisplay()
    {
        _minutes = (int)(ClockGame.Instance.TimerGame / 60);
        if (_minutes >= 60) {
            _hour = _minutes / 60;
            _minutes -= 60 * _hour;
        }
        _seconds = Mathf.FloorToInt(ClockGame.Instance.TimerGame % 60);
        if (_hour == 0 && _minutes == 0)
            _timePassed.text = string.Format("{0,00}sec.", _seconds);
        else if(_hour == 0 && _minutes > 0)
            _timePassed.text = string.Format("{0,00:00}min. {1,1:00}sec.",_minutes,_seconds);
        else
            _timePassed.text = string.Format("{0,0:0}h. {1,1:00}min. {2,1:00}sec.",_hour,_minutes,_seconds);
    }

    private void SetupMazeCompleteStat()
    {
        _monsterKillCount.text = DataCollector.Instance.Kill.ToString();
        _mazeCompleteDisplay.text = DataCollector.Instance.MazeComplete.ToString();
    }

    private void SetupCharacterStat()
    {
        _playerLevel.text = DataCollector.Instance.PlayerLevel + "/20";
        _playerLifeStat.text = DataCollector.Instance.PlayerHp + "/" + DataCollector.Instance.PlayerMaxHp;
        _playerConstitutionCount.text = DataCollector.Instance.PlayerConstitution.ToString();
        _playerAttackSpeedCount.text = DataCollector.Instance.PlayerSwiftness.ToString();
        _playerPowerCount.text = DataCollector.Instance.PlayerPower.ToString();
    }
    
    public void UpdateDisplay()
    {
        SetupTimeDisplay();
        SetupMazeCompleteStat();
        SetupCharacterStat();
    }

    public void ContinueGame()
    {
        DataCollector.Instance.SaveDataAndContinue();
    }

    public void QuitGame()
    {
        DataCollector.Instance.SaveDataAndLeave();
    }
}
