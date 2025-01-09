using Gameplay.GameData;
using UI;
using UnityEngine;

namespace Gameplay
{
    public class Exit : MonoBehaviour
    {
        [SerializeField] private float _delay;
        [SerializeField] private GameObject[] _teleporteFX;
        [SerializeField] private GameObject _endGameUI;

        private bool _exit;
        private float _timer;

        private void Awake()
        {
            _endGameUI.SetActive(false);
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
            if (_timer >= _delay)
            {
                MazeComplete();
            }
        }

        private void MazeComplete()
        {
            if(DataCollector.Instance)
                DataCollector.OnMazeComplete?.Invoke();
            Time.timeScale = 0;
            ClockGame.Instance.ClockStop();
            DataCollector.Instance.UpdateDataCollector();
            _endGameUI.SetActive(true);
            _endGameUI.transform.GetComponent<EndGameUI>().UpdateDisplay();
            _timer = 0;
        }
        
    }
}