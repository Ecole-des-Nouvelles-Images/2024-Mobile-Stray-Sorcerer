using System;
using System.Collections;
using DG.Tweening;
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

        [Header("Scenes References")]
        [SerializeField] private SceneField _setup;
        [SerializeField] private SceneField _titleScreen;
        [SerializeField] private SceneField _tutorialScene;
        [SerializeField] private SceneField _gameScene;

        public SceneBuilder LoadingBuilder { get; set; }

        public Action OnLaunchGame;

        private SceneField _currentScene;
        private SceneField _loadingScene;

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
            GameObject.Find("UI/Root").GetComponent<CanvasGroup>().DOFade(0, 1).SetUpdate(true).OnComplete(() =>
            {
                OnLaunchGame?.Invoke();
                StartCoroutine(LoadCoroutine(_gameScene, true));
            });
        }

        public void ReloadGameScene()
        {
            OnLaunchGame?.Invoke();
            StartCoroutine(ReloadGameSceneCoroutine());
        }

        private IEnumerator LoadCoroutine(SceneField scene, bool isGameScene)
        {
            _loadingScreen.Show(true);

            yield return LoadSceneCoroutine(scene);

            _loadingScene = scene;
            _loadingScene.Scene.GetRootGameObjects();

            if (isGameScene)
            {
                while (!LoadingBuilder) yield return null;

                SceneManager.SetActiveScene(_loadingScene);

                yield return LoadingBuilder.Build(_loadingScreen);
            }

            if (_currentScene != null)
                yield return UnloadSceneCoroutine(_currentScene);

            _loadingScreen.Show(false);
            _currentScene = _loadingScene;
        }

        private IEnumerator LoadSceneCoroutine(SceneField scene)
        {
            float minimumTimer = 0f;
            AsyncOperation asyncLoadOperation = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

            if (asyncLoadOperation == null)
                throw new NullReferenceException($"LoadSceneAsync error: {scene} scene is null.");

            asyncLoadOperation.allowSceneActivation = false;

            while (asyncLoadOperation.progress < 0.9f && minimumTimer < _minimumLoadTime)
            {
                minimumTimer += Time.deltaTime;

                yield return null;
            }

            asyncLoadOperation.allowSceneActivation = true;
        }

        private IEnumerator UnloadSceneCoroutine(SceneField scene)
        {
            AsyncOperation asyncUnloadOperation = SceneManager.UnloadSceneAsync(scene.Scene);

            if (asyncUnloadOperation == null)
                throw new NullReferenceException($"UnloadSceneAsync error: {scene} scene is null.");

            while (!asyncUnloadOperation.isDone)
            {
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

        public GameObject SceneUtilityActivatePlayer(GameObject prefab, Vector3 position)
        {
            SceneManager.SetActiveScene(_setup);
            GameObject player = Instantiate(prefab, position, Quaternion.identity);
            player.name = "Player";
            SceneManager.SetActiveScene(_gameScene);

            return player;
        }
    }
}