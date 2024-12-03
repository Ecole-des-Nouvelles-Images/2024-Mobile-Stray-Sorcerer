using System.Collections;
using MazeGenerator;
using UI;
using UnityEngine;

namespace Manager
{
    public class SceneBuilder: MonoBehaviour
    {
        [SerializeField] private MazeBuilder _maze;
        // [SerializeField] private FoesManager _foesManager;

        private LoadingScreen _loadingScreen;

        private void Awake()
        {
            try {
                SceneLoader loader = GameObject.FindWithTag("Loader").GetComponent<SceneLoader>();
                loader.LoadingBuilder = this;
            }
            catch {
                Debug.LogError("Caution: Avoid loading this scene directly. Use the SceneLoader instead.");
                throw;
            }
        }

        public IEnumerator Build(LoadingScreen loadingScreen)
        {
            _loadingScreen = loadingScreen;

            yield return StartCoroutine(BuildMaze());

            yield return StartCoroutine(GenerateProps());

            yield return StartCoroutine(InstantiateFoes());

            yield return StartCoroutine(BuildNavMesh());

            GameManager.Instance.StartGame();

            // yield return StartCoroutine(BakeLighting());

            // yield return StartCoroutine(ComputeOcclusionCullingData());
        }

        private IEnumerator BuildMaze()
        {
            _loadingScreen.UpdateStatus("> Building maze...");
            yield return StartCoroutine(_maze.Build());
        }

        private IEnumerator GenerateProps()
        {
            _loadingScreen.UpdateStatus("> (Simulating) Generating props..");
            yield return new WaitForSeconds(2);
        }

        private IEnumerator InstantiateFoes()
        {
            _loadingScreen.UpdateStatus("> (Simulating) Instantiating foes...");
            yield return new WaitForSeconds(2);
        }

        private IEnumerator BuildNavMesh()
        {
            _loadingScreen.UpdateStatus("> Building NavMesh...");
            yield return StartCoroutine(_maze.InitializeNavMesh(true));
        }

        private IEnumerator BakeLighting()
        {
            _loadingScreen.UpdateStatus("> (Simulating) Baking lights...");
            yield return new WaitForSeconds(2);
        }

        private IEnumerator ComputeOcclusionCullingData()
        {
            // Maybe not useful (Editor-only)
            yield break;
        }
    }
}