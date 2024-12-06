using System;
using Manager;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TitleTouchToPlayButton: MonoBehaviour
    {
        private Button _touchToPlay;

        private void OnEnable()
        {
            SceneLoader loader = null;

            SceneLoader.Instance.OnLaunchGame += DisableButton;

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

            _touchToPlay = GetComponent<Button>();
            _touchToPlay.onClick.AddListener(loader.LaunchGame);
        }

        private void OnDisable()
        {
            SceneLoader.Instance.OnLaunchGame -= DisableButton;
        }

        private void DisableButton()
        {
            _touchToPlay.interactable = false;
        }
    }
}