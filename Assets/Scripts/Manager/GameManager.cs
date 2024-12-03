using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using Utils;

namespace Manager
{
    public class GameManager: SingletonMonoBehaviour<GameManager>
    {
        [SerializeField] private GameObject _camera;
        [SerializeField] private GameObject _interfaceOverlay;

        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private Vector3 _playerSpawnPosition;

        public void StartGame()
        {
            _camera.SetActive(true);
            _interfaceOverlay.SetActive(true);

            GameObject player = Instantiate(_playerPrefab, _playerSpawnPosition, Quaternion.identity);
            player.GetComponent<PlayerInput>().uiInputModule = FindObjectOfType<InputSystemUIInputModule>();
        }
    }
}