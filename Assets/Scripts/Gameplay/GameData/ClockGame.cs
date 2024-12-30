using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Utils;

namespace Gameplay.GameData
{
    public class ClockGame : SingletonMonoBehaviour<ClockGame>
    {
        public static Action<int> OnMonstersGrow;
        
        public int GrowingLevel;
        public float TimerGame;
        
        [SerializeField] private int _growingTime;

        private List<TMP_Text> _timerTextDisplay = new List<TMP_Text>();
        private float _timerGame;
        private bool _isGameStart;
        private int _hour;
        private int _minutes;
        private int _seconds;

        private void Awake()
        {
            Reset();
            GrowingLevel = 0;
        }

        private void Update()
        {
            if (_isGameStart)
                TimerGame += Time.deltaTime;
            if (TimerGame > _growingTime && TimerGame / _growingTime > GrowingLevel)
            {
                GrowingLevel += 1;
                OnMonstersGrow?.Invoke(GrowingLevel);
            }
            _minutes = (int)(TimerGame / 60);
            if (_minutes >= 60) {
                _hour = _minutes / 60;
                _minutes -= 60 * _hour;
            }
            _seconds = Mathf.FloorToInt(TimerGame % 60);
            if (_timerTextDisplay.Count > 0)
            {
                foreach (TMP_Text text in _timerTextDisplay)
                {
                    if (_hour == 0 && _minutes == 0)
                        text.text = string.Format("{0,00}", _seconds);
                    else if(_hour == 0 && _minutes > 0)
                        text.text = string.Format("{0,00:00}:{1,1:00}",_minutes,_seconds);
                    else
                        text.text = string.Format("{0,0:0}:{1,1:00}:{2,1:00}",_hour,_minutes,_seconds);
                }
            }
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
        public void ClockStart()
        {
            _isGameStart = true;
        }
        public void ClockStop()
        {
            _isGameStart = false;
        }

        public void AddDisplayClock(TMP_Text textDisplay)
        {
            foreach (TMP_Text display in _timerTextDisplay)
            {
                if (display == textDisplay)
                    return;
            }
            _timerTextDisplay.Add(textDisplay);
        }
        public void RemoveDisplayClock(TMP_Text textDisplay)
        {
            foreach (TMP_Text display in _timerTextDisplay.ToList())
            {
                if (display == textDisplay)
                    _timerTextDisplay.Remove(textDisplay);
            }
        }
    }
}