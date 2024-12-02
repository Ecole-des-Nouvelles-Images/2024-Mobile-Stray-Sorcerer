using System;
using System.Collections;
using UI;
using UnityEngine;

namespace Manager
{
    public class SceneBuilder: MonoBehaviour
    {
        private LoadingScreen _loadingScreen;

        public static SceneBuilder FindFrom(GameObject[] sceneRootObjects)
        {
            foreach (GameObject rootObject in sceneRootObjects)
            {
                SceneBuilder sceneBuilder = rootObject.GetComponent<SceneBuilder>();

                if (sceneBuilder)
                    return sceneBuilder;
            }

            throw new NullReferenceException("Error: SceneBuilder not found in scene.");
        }

        public IEnumerator Build(LoadingScreen loadingScreen)
        {
            _loadingScreen = loadingScreen;

            yield return StartCoroutine(BuildMaze());

            yield return StartCoroutine(GenerateProps());

            yield return StartCoroutine(InstantiateFoes());

            yield return StartCoroutine(BakeLighting());

            yield return StartCoroutine(BuildNavMesh());

            // yield return StartCoroutine(ComputeOcclusionCullingData());
        }

        private IEnumerator BuildMaze()
        {
            _loadingScreen.UpdateStatus("> (Simulating) Building maze...");
            yield return new WaitForSeconds(1.5f);
        }

        private IEnumerator GenerateProps()
        {
            _loadingScreen.UpdateStatus("> (Simulating) Generating props..");
            yield return new WaitForSeconds(1.5f);
        }

        private IEnumerator InstantiateFoes()
        {
            _loadingScreen.UpdateStatus("> (Simulating) Instantiating foes...");
            yield return new WaitForSeconds(1.5f);
        }

        private IEnumerator BakeLighting()
        {
            _loadingScreen.UpdateStatus("> (Simulating) Baking lights...");
            yield return new WaitForSeconds(1.5f);
        }

        private IEnumerator BuildNavMesh()
        {
            _loadingScreen.UpdateStatus("> (Simulating) Building NavMesh...");
            yield return new WaitForSeconds(1.5f);
        }

        private IEnumerator ComputeOcclusionCullingData()
        {
            // Maybe not useful (Editor-only)
            yield break;
        }
    }
}