using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gameplay
{
    public class Exit : MonoBehaviour
    {
        [SerializeField] private float _delay;
        [SerializeField] private GameObject _teleporteFX;

        private bool _exit;
        private float _timer;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _exit = true;
                _teleporteFX.SetActive(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _exit = false;
                _teleporteFX.SetActive(false);
                _timer = 0;
            }
        }

        private void Update()
        {
            if (_exit && _timer < _delay)
                _timer += Time.deltaTime;
            if (_timer >= _delay) CallScene();
        }

        private void CallScene()
        {
            SceneManager.LoadScene("Setup");
            _timer = 0;
        }
    }
}