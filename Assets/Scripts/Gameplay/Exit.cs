using System;
using Gameplay.GameSaveDataSystem;
using UnityEngine;

namespace Gameplay
{
    public class Exit : MonoBehaviour
    {
        /// <summary>Raised once the player stayed long enough on the exit teleporter (after save data is updated).</summary>
        public static Action OnMazeExitReached;

        [SerializeField] private float _delay;
        [SerializeField] private GameObject[] _teleporteFX;

        private bool _exit;
        private bool _completed;
        private float _timer;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void OnRuntimeInitialization()
        {
            OnMazeExitReached = null;
        }

        private void Awake()
        {
            foreach (GameObject fx in _teleporteFX)
            {
                fx.SetActive(false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _exit = true;
                foreach (GameObject fx in _teleporteFX)
                {
                    fx.SetActive(true);
                }
                
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _exit = false;
                foreach (GameObject fx in _teleporteFX)
                {
                    fx.SetActive(true);
                }
                _timer = 0;
            }
        }

        private void Update()
        {
            if (_exit && _timer < _delay)
                _timer += Time.deltaTime;
            if (_timer >= _delay && !_completed)
            {
                MazeComplete();
            }
        }

        private void MazeComplete()
        {
            _completed = true;
            ClockGame.Instance.ClockStop();
            Time.timeScale = 0;
            if(DataSaveSystem.Instance)
                DataSaveSystem.OnMazeComplete?.Invoke();
            OnMazeExitReached?.Invoke();
            _timer = 0;
        }
        
    }
}