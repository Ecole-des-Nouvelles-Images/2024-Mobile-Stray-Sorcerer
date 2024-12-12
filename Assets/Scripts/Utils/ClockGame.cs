using System;
using UnityEngine;

namespace Utils
{
    public class ClockGame : SingletonMonoBehaviour<ClockGame>
    {
        [SerializeField] private int _growingTime;
        public float TimerGame
        {
            get => _timerGame;
            private set => _timerGame = value;
        }

        public static Action<int> OnMonstersGrow;
        
        private float _timerGame;
        private bool _isGameStart;
        private int _growingLevel;

        private void Update()
        {
            if(_isGameStart)
                TimerGame += Time.deltaTime;
            if (TimerGame % _growingTime > _growingLevel)
            {
                _growingLevel += 1;
                OnMonstersGrow.Invoke(_growingLevel);
            }
        }

        public void Reset()
        {
            TimerGame = 0;
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