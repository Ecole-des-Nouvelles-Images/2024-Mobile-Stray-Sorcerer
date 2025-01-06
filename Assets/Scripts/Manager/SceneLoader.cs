using System;
using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Manager
{
    public class SceneLoader : SingletonMonoBehaviour<SceneLoader>
    {
        [SerializeField] private LoadingScreen _loadingScreen;
        [SerializeField] private TransitionSystem _transition;

        [Header("Scenes References")] [SerializeField]
        private SceneField _titleScreen;

        [SerializeField] private SceneField _tutorialScene;
        [SerializeField] private SceneField _gameScene;

        public SceneBuilder LoadingBuilder { get; set; }

        public Action OnLaunchGame;

        private SceneField _currentScene = null;
        private SceneField _loadingScene;
        private GameObject[] _loadingSceneRootObjects;

        // DEBUG
        private float _minimumLoadTime = 3f;

        private void Awake()
        {
            SceneManager.LoadScene(_titleScreen, LoadSceneMode.Additive);
            _currentScene = _titleScreen;
        }

        public void LoadTitleScreen()
        {
            StartCoroutine(LoadCoroutine(_titleScreen, false));
        }

        public void LaunchGame()
        {
            OnLaunchGame?.Invoke();
            StartCoroutine(LoadCoroutine(_gameScene, true));
        }
        public void ReloadGameScene()
        {
            OnLaunchGame?.Invoke();
            StartCoroutine(ReloadGameSceneCoroutine());
        }

        private IEnumerator LoadCoroutine(SceneField scene, bool isGameScene)
        {
            yield return LoadSceneCoroutine(scene);

            if (isGameScene)
            {
                _loadingScene = scene;
                _loadingSceneRootObjects = _loadingScene.Scene.GetRootGameObjects();

                while (!LoadingBuilder) yield return null;

                yield return LoadingBuilder.Build(_loadingScreen);
            }

            if (_currentScene != null)
                yield return UnloadSceneCoroutine(_currentScene);


            _loadingScreen.Show(false);
            _currentScene = _loadingScene;
            SceneManager.SetActiveScene(_currentScene);
        }

        private IEnumerator LoadSceneCoroutine(SceneField scene)
        {
            float minimumTimer = 0f;
            AsyncOperation asyncLoadOperation = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

            if (asyncLoadOperation == null)
                throw new NullReferenceException($"LoadSceneAsync error: {scene} scene is null.");

            asyncLoadOperation.allowSceneActivation = false;
            _loadingScreen.Show(true);

            while (asyncLoadOperation.progress < 0.9f && minimumTimer < _minimumLoadTime)
            {
                _loadingScreen.UpdateStatus($"> Loading {scene} scene... {asyncLoadOperation.progress * 100}%");
                minimumTimer += Time.deltaTime;

                yield return null;
            }

            _loadingScreen.UpdateStatus($"> Activating {scene} scene");
            asyncLoadOperation.allowSceneActivation = true;
        }

        private IEnumerator UnloadSceneCoroutine(SceneField scene)
        {
            AsyncOperation asyncUnloadOperation = SceneManager.UnloadSceneAsync(scene.Scene);

            if (asyncUnloadOperation == null)
                throw new NullReferenceException($"UnloadSceneAsync error: {scene} scene is null.");

            while (!asyncUnloadOperation.isDone)
            {
                _loadingScreen.UpdateStatus($"> Unloading resources... {asyncUnloadOperation.progress * 100}%");
                yield return null;
            }
        }
        
        private IEnumerator ReloadGameSceneCoroutine()
        {
            Debug.Log($"Current scene <{_currentScene}> should be Game");
    
            _loadingScreen.Show(true);
    
            yield return UnloadSceneCoroutine(_currentScene);

            yield return LoadSceneCoroutine(_gameScene);
    
            while (!LoadingBuilder) yield return null;

            yield return LoadingBuilder.Build(_loadingScreen);

            _loadingScreen.Show(false);
            _currentScene = _gameScene;
            SceneManager.SetActiveScene(_currentScene);
        }
    }
}