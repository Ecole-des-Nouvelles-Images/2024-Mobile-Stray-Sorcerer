﻿using System;
using TMPro;
using UnityEngine;

namespace Utils
{
    public class ClockGame : SingletonMonoBehaviour<ClockGame>
    {
        [SerializeField] private TMP_Text _timerTextDisplay;
        [SerializeField] private int _growingTime;

        public int GrowingLevel { get; private set; }
        public float TimerGame
        {
            get => _timerGame;
            private set => _timerGame = value;
        }

        public static Action<int> OnMonstersGrow;

        private float _timerGame;
        private bool _isGameStart;
        private int _hour;
        private int _minutes;
        private int _seconds;

        private void Update()
        {
            if (_isGameStart)
                TimerGame += Time.deltaTime;
            if (TimerGame / _growingTime > GrowingLevel)
            {
                GrowingLevel += 1;
                OnMonstersGrow.Invoke(GrowingLevel);
            }
            _minutes = (int)(TimerGame / 60);
            if (_minutes >= 60) {
                _hour = _minutes / 60;
                _minutes -= 60 * _hour;
            }
            _seconds = Mathf.FloorToInt(TimerGame % 60);
            if (_hour == 0 && _minutes == 0)
                _timerTextDisplay.text = string.Format("{0,00}", _seconds);
            else if(_hour == 0 && _minutes > 0)
                _timerTextDisplay.text = string.Format("{0,00:00}:{1,1:00}",_minutes,_seconds);
            else
                _timerTextDisplay.text = string.Format("{0,0:0}:{1,1:00}:{2,1:00}",_hour,_minutes,_seconds);
        }
        
        public void AddTime()
        {
            TimerGame += 30;
        }

        public void Reset()
        {
            TimerGame = 0;
            GrowingLevel = 0;
        }

        public void GameStart()
        {
            Reset();
            _isGameStart = true;
        }

        public void GameEnd()
        {
            _isGameStart = false;
        }
    }
}