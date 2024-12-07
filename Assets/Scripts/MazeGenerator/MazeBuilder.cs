using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UI;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

#if UNITY_EDITOR
using Unity.EditorCoroutines.Editor;
#endif

using Utils;

namespace MazeGenerator
{
    public class MazeBuilder : SingletonMonoBehaviour<MazeBuilder>
    {
        [Header("System")]
        [SerializeField] private NavMeshSurface _navMesh;

        [Header("References")]
        [SerializeField] private List<CellRule> _cellRules;

        [Header("Settings")]
        [SerializeField] private string _seedPhrase;
        [SerializeField] private int _scale = 1;

        private const int _CELL_SIZE = 20;

        public int Seed
        {
            get
            {
                if (_seed != _seedPhrase)
                {
                    _seed = _seedPhrase;
                    _hashSeed = HashSeed(_seed);
                }

                return _hashSeed;
            }
        }

        private string _seed;
        private int _hashSeed;
        private Maze _maze;

        public IEnumerator Build()
        {
            Clear();

            _maze = new Maze(_scale);
            _maze.Generate();

            for (int y = 0; y < _maze.Scale; y++)
            {
                for (int x = 0; x < _maze.Scale; x++)
                {
                    Cell cell = _maze.GetCell(x, y);

                    if (cell != null)
                        Instantiate(cell.Prefab, new Vector3(x * _CELL_SIZE, 0, y * _CELL_SIZE), Quaternion.identity, transform);
                    else
                        throw new Exception("Error: invalid grid, null cell found.");

                    yield return null;
                }
            }
        }

        public IEnumerator InitializeNavMesh(LoadingScreen loadingScreen, bool forceRebuild = false)
        {
            Bounds mazeBounds = new (new Vector3(_scale * _CELL_SIZE / 2f, 0, _scale * _CELL_SIZE / 2f), Vector3.one * ((_scale + 1) * _CELL_SIZE));
            List<NavMeshBuildSource> navMeshSources = new();
            loadingScreen.UpdateStatus("> Building NavMesh... [Gathering modifiers...]");
            List<NavMeshBuildMarkup> navMeshMarkups = GetNavMeshBuildModifiers();
            NavMeshBuildSettings navMeshBuildSettings;

            loadingScreen.UpdateStatus("> Building NavMesh... [Gathering sources...]");
            NavMeshBuilder.CollectSources(transform, LayerMask.GetMask("Default"), NavMeshCollectGeometry.RenderMeshes, 0, navMeshMarkups, navMeshSources);

            yield return null;

            loadingScreen.UpdateStatus("> Building NavMesh... [Recover settings...]");
            int navAgentTypes = NavMesh.GetSettingsCount();
            NavMeshData[] navMeshData = new NavMeshData[navAgentTypes];

            for (int navAgentIndex = 0; navAgentIndex < navAgentTypes; navAgentIndex++)
            {
                navMeshBuildSettings = NavMesh.GetSettingsByIndex(navAgentIndex);

                foreach (string s in navMeshBuildSettings.ValidationReport(mazeBounds)) {
                    Debug.LogWarning($"NavMeshBuildSettings validation report: {s}");
                }

                loadingScreen.UpdateStatus("> Building NavMesh... [Applying overrides...]");
                // Override Settings
                navMeshBuildSettings.overrideVoxelSize = true;
                navMeshBuildSettings.voxelSize = 0.2f;
                navMeshBuildSettings.overrideTileSize = true;
                navMeshBuildSettings.tileSize = 256;

                navMeshData[navAgentIndex] = new NavMeshData();
                AsyncOperation asyncNavMeshBuild = NavMeshBuilder.UpdateNavMeshDataAsync(navMeshData[navAgentIndex], navMeshBuildSettings, navMeshSources, mazeBounds);

                while (!asyncNavMeshBuild.isDone) yield return null;
            }

            if (forceRebuild)
                NavMesh.RemoveAllNavMeshData();

            loadingScreen.UpdateStatus("> Building NavMesh... [Generating data...]");
            for (int dataIndex = 0; dataIndex < navMeshData.Length; dataIndex++)
                NavMesh.AddNavMeshData(navMeshData[dataIndex]);

            yield return null;
        }

        #region Utils

        private int HashSeed(string seed)
        {
            using (SHA1 sha1 = new SHA1Managed())
            {
                byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(seed));
                return BitConverter.ToInt32(hash, 0);
            }
        }

        private List<NavMeshBuildMarkup> GetNavMeshBuildModifiers()
        {
            NavMeshModifier[] navMeshModifiers = GetComponentsInChildren<NavMeshModifier>(true);
            List<NavMeshBuildMarkup> navMeshBuildMarkups = new();

            foreach (NavMeshModifier mod in navMeshModifiers)
            {
                NavMeshBuildMarkup markup = new NavMeshBuildMarkup
                {
                    root = mod.transform,
                    area = 1,
                };
                navMeshBuildMarkups.Add(markup);
            }

            return navMeshBuildMarkups;
        }

        public GameObject GetCellPrefab(Cell cell)
        {
            foreach (CellRule rule in _cellRules)
            {
                if (rule.Top == cell.WallTop && rule.Right == cell.WallRight && rule.Bottom == cell.WallBottom && rule.Left == cell.WallLeft)
                {
                    if (rule.Prefab != null)
                        return rule.Prefab;

                    throw new NullReferenceException($"Error in cell rules, no prefab for the following rule [Top: {(cell.WallTop ? "\u2714" : "\u2716")}, Right: {(cell.WallRight ? "\u2714" : "\u2716")}, Bottom: {(cell.WallBottom ? "\u2714" : "\u2716")}, Left: {(cell.WallLeft ? "\u2714" : "\u2716")}]");
                }
            }

            throw new Exception("Discrepancy in cell rules: unknown cell configuration found");
        }

#if UNITY_EDITOR

        [ContextMenu("[EDITOR] Build maze")]
        public void DebugBuild()
        {
            EditorCoroutineUtility.StartCoroutine(Build(), this);
        }

#endif

        [ContextMenu("[EDITOR] Clear maze")]
        private void Clear()
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }

        #endregion
    }
}