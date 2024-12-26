using UnityEngine;
using Utils;

namespace Gameplay.GameData
{
    public class DataCollector : SingletonMonoBehaviour<DataCollector>
    {
        private GameObject _playerPrefabs;
        private ClockGame _clockGame;
        private int _kill;
        private int _mazeFinished;
        
        

        private void Awake()
        {
            _clockGame = ClockGame.Instance;
        }

        public void IncrementKill()
        {
            _kill++;
        }
        public void IncrementMazeFished()
        {
            _mazeFinished++;
        }
    }
}
