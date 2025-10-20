using System;
using Manager;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TitleTouchToPlayButton : MonoBehaviour
    {
        private Button _touchToPlay;
        private float _timerBeforInteractable;

        private void OnEnable()
        {
            SceneLoader loader = null;

            try
            {
                loader = GameObject.FindWithTag("Loader").GetComponent<SceneLoader>();
            }
            catch (NullReferenceException)
            {
                Debug.LogError("Error: SceneLoader not found in scene. Load the scene from /Scenes/Setup.unity");
                Debug.LogWarning("TouchToPlay button's callback has not been set up.");
                return;
            }

            _touchToPlay.interactable = false;
            _touchToPlay = GetComponent<Button>();
            _touchToPlay.onClick.AddListener(loader.LaunchGame);
            _touchToPlay.onClick.AddListener(DisableButton);
        }

        private void OnDisable()
        {
            _touchToPlay.onClick.RemoveAllListeners();
        }

        private void Awake()
        {
            _touchToPlay = GetComponent<Button>();
            _timerBeforInteractable = 3f;
        }

        private void Update()
        {
            if (_timerBeforInteractable > 0)
            {
                _timerBeforInteractable -= Time.deltaTime;
            }
            else
            {
                _touchToPlay.interactable = true;
            }
        }

        private void DisableButton()
        {
            _touchToPlay.onClick.RemoveAllListeners();
            _touchToPlay.interactable = false;
        }
    }
}