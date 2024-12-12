using UnityEngine;
using UnityEngine.SceneManagement;

namespace Maze
{
    public class Exit : MonoBehaviour
    {
        [SerializeField] private float _delay;

        private bool _exit;
        private float _timer;
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _exit = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _exit = false;
                _timer = 0;
            }
        }

        private void Update()
        {
            if(_exit && _timer < _delay)
                _timer += Time.deltaTime;
            if (_timer >= _delay)
            {
                CallScene();
            }
        }

        private void CallScene()
        {
            SceneManager.LoadScene("setup");
            _timer = 0;
        }
    }
}
