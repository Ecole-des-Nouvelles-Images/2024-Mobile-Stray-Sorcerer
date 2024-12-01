using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Manager
{
    public class SceneLoader : SingletonMonoBehaviour<SceneLoader>
    {
        [Header("Scenes References")]
        [SerializeField] private SceneField _titleScreen;
        [SerializeField] private SceneField _tutorialScene;
        [SerializeField] private SceneField _gameScene;

        private int CurrentSceneIndex => SceneManager.GetActiveScene().buildIndex;

        private void Awake()
        {
            SceneManager.LoadScene(_titleScreen, LoadSceneMode.Additive);
        }

        public void LoadTutorial()
        {
            StartCoroutine(LoadSceneCoroutine(_tutorialScene));
        }

        private IEnumerator LoadSceneCoroutine(string sceneName)
        {
            string progressStatus;
            AsyncOperation asyncLoadOperation = SceneManager.LoadSceneAsync(_tutorialScene, LoadSceneMode.Additive);

            if (asyncLoadOperation == null)
                throw new NullReferenceException($"LoadSceneAsync error: {_tutorialScene.SceneName} scene is null.");

            // Enable loading screen

            while (!asyncLoadOperation.isDone)
            {
                progressStatus = $"> Loading {sceneName} scene... {asyncLoadOperation.progress * 100}%";
                // Update loading screen;
                yield return null;
            }

            // Activate tutorial scene
        }
    }
}