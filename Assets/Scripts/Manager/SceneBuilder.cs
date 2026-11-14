using System.Collections;
using AI;
using Maze;
using UI;
using UnityEngine;

namespace Manager
{
    public class SceneBuilder : MonoBehaviour
    {
        [SerializeField] private MazeBuilder _maze;
        // [SerializeField] private FoesManager _foesManager;

        private LoadingScreen _loadingScreen;

        private void Awake()
        {
            try
            {
                SceneLoader loader = GameObject.FindWithTag("Loader").GetComponent<SceneLoader>();
                loader.LoadingBuilder = this;
            }
            catch
            {
                Debug.LogError("Caution: Avoid loading this scene directly. Use the SceneLoader instead.");
                throw;
            }
        }

        public IEnumerator Build(LoadingScreen loadingScreen)
        {
            _loadingScreen = loadingScreen;

            yield return BuildMaze();

            yield return GenerateMazeEnds();

            yield return GenerateProps();

            yield return BuildNavMesh();

            yield return InstantiateFoes();

            _loadingScreen.UpdateLog("LAUNCHING_GAME");

            GameManager.Instance.StartGame();

            _loadingScreen.Show(false);

            // yield return StartCoroutine(BakeLighting());

            // yield return StartCoroutine(ComputeOcclusionCullingData());
        }


        private IEnumerator BuildMaze()
        {
            _loadingScreen.UpdateLog(
                "BUILDING_MAZE", $"<size=70%>{{{_maze.SeedPhrase}}}</size>");
            yield return _maze.Build();
        }

        private IEnumerator GenerateMazeEnds()
        {
            yield return _maze.DefineEntryAndExit();
        }

        private IEnumerator GenerateProps()
        {
            _loadingScreen.UpdateLog("ADDING_DECORATIONS");
            yield return _maze.GenerateProps();
        }

        private IEnumerator InstantiateFoes()
        {
            _loadingScreen.UpdateLog("SPAWNING_MONSTERS");
            yield return SquadDistributor.Instance.SquadsDistributionInLab();
        }

        private IEnumerator BuildNavMesh()
        {
            _loadingScreen.UpdateLog("CONFIGURING_NAVMESH");
            yield return _maze.InitializeNavMesh(_loadingScreen, true);
        }

        private IEnumerator BakeLighting()
        {
            yield return new WaitForSeconds(2);
        }

        private IEnumerator ComputeOcclusionCullingData()
        {
            // Maybe not useful (Editor-only)
            yield break;
        }
    }
}
