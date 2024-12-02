using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils
{
    [System.Serializable]
    public class SceneField
    {
        [SerializeField]
        private Object _sceneAsset;

        [SerializeField]
        private string _sceneName = "";

        public string SceneName => _sceneName;
        public Scene Scene => SceneManager.GetSceneByName(_sceneName);

        // makes it work with the existing Unity methods (LoadLevel/LoadScene)
        public static implicit operator string (SceneField sceneField)
        {
            return sceneField.SceneName;
        }

        public static implicit operator SceneField(Scene scene)
        {
            return new SceneField { _sceneName = scene.name };
        }

        public static implicit operator Scene(SceneField sceneField)
        {
            return sceneField.Scene;
        }
    }
}