using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UI;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using Utils;
using Random = System.Random;

#if UNITY_EDITOR
using Unity.EditorCoroutines.Editor;
#endif


namespace Maze
{
    [RequireComponent(typeof(NavMeshSurface))]
    public class MazeBuilder : SingletonMonoBehaviour<MazeBuilder>
    {
        [Header("References")] [SerializeField]
        private List<CellRule> _cellRules;

        [SerializeField] private GameObject _entryPrefab;
        [SerializeField] private GameObject _exitPrefab;

        [Header("Generation settings")] [SerializeField]
        private bool _useRandomSeed;

        [SerializeField] private string _seedPhrase;
        [SerializeField] private int _scale = 1;

        [Header("Props Prefabs")] [SerializeField]
        private GameObject _torchPrefab;

        [SerializeField] private List<GameObject> _mushroomsPrefabs;

        [Header("Props repartition")] [SerializeField] [Range(0, 1)]
        private float _lightEmitterProbabilityPerSlot = 0.5f;

        [SerializeField] private int _maxLightEmittersPerCell = 4;
        // [SerializeField] private int _maxPropsPerCell = 4;

        public GameObject[,] MazeCells { get; private set; }
        public int Scale => _scale;

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

        private const int _CELL_SIZE = 20;

        private void Awake()
        {
            if (_useRandomSeed)
                _seedPhrase = GenerateRandomSeed();
        }

        public IEnumerator Build()
        {
            Clear();

            MazeCells = new GameObject[_scale, _scale];
            _maze = new Maze(_scale);
            _maze.Generate();

            for (int y = 0; y < _maze.Scale; y++)
            for (int x = 0; x < _maze.Scale; x++)
            {
                Cell cell = _maze.GetCell(x, y);

                if (cell != null)
                    MazeCells[x, y] = Instantiate(cell.Prefab, new Vector3(x * _CELL_SIZE, 0, y * _CELL_SIZE), Quaternion.identity, transform);
                else
                    throw new Exception("Error: invalid grid, null cell found.");

                yield return null;
            }
        }

        public IEnumerator DefineEntryAndExit()
        {
            Vector2Int exitCellPos = _maze.ExitCell.Position;
            GameObject entry = MazeCells[0, 0];
            GameObject exit = MazeCells[exitCellPos.x, exitCellPos.y];

            List<GameObject> objectsToSanitize = new();

            Instantiate(_entryPrefab, MazeCells[0, 0].transform.position, Quaternion.identity, MazeCells[0, 0].transform);
            Instantiate(_exitPrefab, MazeCells[exitCellPos.x, exitCellPos.y].transform.position, Quaternion.identity, MazeCells[exitCellPos.x, exitCellPos.y].transform);

            objectsToSanitize.AddRange(GetChildrenObjectsFrom(entry.transform.Find("PropsAnchors")).ToList());
            objectsToSanitize.AddRange(GetChildrenObjectsFrom(entry.transform.Find("LightEmittersAnchors")).ToList());

            objectsToSanitize.AddRange(GetChildrenObjectsFrom(exit.transform.Find("PropsAnchors")).ToList());
            objectsToSanitize.AddRange(GetChildrenObjectsFrom(exit.transform.Find("LightEmittersAnchors")).ToList());

            for (int i = 0; i < objectsToSanitize.Count; i++) Destroy(objectsToSanitize[i].gameObject);

            yield return null;
        }

        public IEnumerator InitializeNavMesh(LoadingScreen loadingScreen, bool forceRebuild = false)
        {
            Bounds mazeBounds = new(new Vector3(_scale * _CELL_SIZE / 2f, 0, _scale * _CELL_SIZE / 2f), Vector3.one * ((_scale + 1) * _CELL_SIZE));
            List<NavMeshBuildSource> navMeshSources = new();
            loadingScreen.UpdateStatus("> Building NavMesh... [Gathering modifiers...]");
            List<NavMeshBuildMarkup> navMeshMarkups = GetNavMeshBuildModifiers();
            NavMeshBuildSettings navMeshBuildSettings;

            loadingScreen.UpdateStatus("> Building NavMesh... [Gathering sources...]");
            NavMeshBuilder.CollectSources(transform, LayerMask.GetMask("Wall"), NavMeshCollectGeometry.RenderMeshes, 0, navMeshMarkups, navMeshSources);

            yield return null;

            loadingScreen.UpdateStatus("> Building NavMesh... [Recover settings...]");
            int navAgentTypes = NavMesh.GetSettingsCount();
            NavMeshData[] navMeshData = new NavMeshData[navAgentTypes];

            for (int navAgentIndex = 0; navAgentIndex < navAgentTypes; navAgentIndex++)
            {
                navMeshBuildSettings = NavMesh.GetSettingsByIndex(navAgentIndex);

                foreach (string s in navMeshBuildSettings.ValidationReport(mazeBounds)) Debug.LogWarning($"NavMeshBuildSettings validation report: {s}");

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

        public IEnumerator GenerateProps()
        {
            Random generator = new(Seed);
            Transform[] torchSlots;
            Transform[] mushroomSlots;
            Transform[] propsSlots;

            for (int y = 0; y < MazeCells.GetLength(1); y++)
            for (int x = 0; x < MazeCells.GetLength(0); x++)
            {
                GameObject cell = MazeCells[x, y];
                torchSlots = GetChildrenFrom(cell.transform.Find("LightEmittersAnchors/Torches"));
                mushroomSlots = GetChildrenFrom(cell.transform.Find("LightEmittersAnchors/LuminescentMushrooms"));
                propsSlots = GetChildrenFrom(cell.transform.Find("PropsAnchors"));

                InstantiateLights(generator, torchSlots, mushroomSlots);
                InstantiateProps(generator, propsSlots);

                yield return null;
            }
        }

        private void InstantiateLights(Random generator, Transform[] torchSlots, Transform[] mushroomSlots)
        {
            if (torchSlots == null && mushroomSlots == null) return;

            Transform[] emittersSlots = Array.Empty<Transform>();
            int lightSlotsCount = 0;

            if (torchSlots != null)
                emittersSlots = emittersSlots.Concat(torchSlots).ToArray();

            if (mushroomSlots != null)
                emittersSlots = emittersSlots.Concat(mushroomSlots).ToArray();

            generator.Shuffle(emittersSlots);

            foreach (Transform slot in emittersSlots)
            {
                if (lightSlotsCount >= _maxLightEmittersPerCell) break;

                if (!(generator.NextDouble() > _lightEmitterProbabilityPerSlot)) continue;

                switch (slot.tag)
                {
                    case "Torch":
                        Instantiate(_torchPrefab, slot);
                        break;
                    case "Mushroom":
                        Instantiate(_mushroomsPrefabs[generator.Next(_mushroomsPrefabs.Count)], slot);
                        break;
                    default:
                        throw new Exception($"Props generation error: unknown light emitter of tag {slot.tag}");
                }

                lightSlotsCount++;
            }
        }

        private void InstantiateProps(Random generator, Transform[] propsAnchors)
        {
            if (propsAnchors == null) return;

            generator.Shuffle(propsAnchors);

            List<(Transform anchor, Props data)> propsSlots = new();

            foreach (Transform slot in propsAnchors)
                propsSlots.Add(new ValueTuple<Transform, Props>(slot, slot.GetComponent<Props>()));

            foreach ((Transform anchor, Props data) slot in propsSlots)
            {
                if (!slot.data) throw new NullReferenceException($"Props generation error: no Props component found on anchor in {slot.anchor.parent.parent.name}");

                slot.data.UpdateDictionary();

                foreach (Enum flag in Props.GetFlags(slot.data.PropType))
                {
                    if (!slot.data.Prefabs.ContainsKey(flag.ToString())) continue;

                    float propsProbability = slot.data.UseCustomProbabilities ? slot.data.Prefabs[flag.ToString()].probability : Props.GLOBAL_PROBABILITY_PER_SLOT;

                    if (generator.NextDouble() < propsProbability)
                    {
                        if (slot.data.Prefabs[flag.ToString()].list.Count > 0)
                        {
                            GameObject propsVariantPrefab = slot.data.Prefabs[flag.ToString()].list[generator.Next(slot.data.Prefabs[flag.ToString()].list.Count)];
                            Instantiate(propsVariantPrefab, slot.anchor);
                            break;
                        }
                    }
                }
            }
        }

        #region Utils

        private string GenerateRandomSeed(int length = 32)
        {
            char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[4 * 32];
            using (RandomNumberGenerator crypto = RandomNumberGenerator.Create())
            {
                crypto.GetBytes(data);
            }

            StringBuilder result = new(length);
            for (int i = 0; i < length; i++)
            {
                uint rnd = BitConverter.ToUInt32(data, i * 4);
                long idx = rnd % chars.Length;

                result.Append(chars[idx]);
            }

            return result.ToString();
        }

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
                NavMeshBuildMarkup markup = new()
                {
                    root = mod.transform,
                    area = 1
                };
                navMeshBuildMarkups.Add(markup);
            }

            return navMeshBuildMarkups;
        }

        public GameObject GetCellPrefab(Cell cell)
        {
            foreach (CellRule rule in _cellRules)
                if (rule.Top == cell.WallTop && rule.Right == cell.WallRight && rule.Bottom == cell.WallBottom && rule.Left == cell.WallLeft)
                {
                    if (rule.Prefab != null)
                        return rule.Prefab;

                    throw new NullReferenceException($"Error in cell rules, no prefab for the following rule [Top: {(cell.WallTop ? "\u2714" : "\u2716")}, Right: {(cell.WallRight ? "\u2714" : "\u2716")}, Bottom: {(cell.WallBottom ? "\u2714" : "\u2716")}, Left: {(cell.WallLeft ? "\u2714" : "\u2716")}]");
                }

            throw new Exception("Discrepancy in cell rules: unknown cell configuration found");
        }

        public Transform[] GetChildrenFrom(Transform obj)
        {
            if (!obj)
                return null;

            Transform[] children = new Transform[obj.childCount];

            for (int i = 0; i < obj.childCount; i++) children[i] = obj.GetChild(i);

            return children;
        }

        public GameObject[] GetChildrenObjectsFrom(Transform obj)
        {
            if (!obj)
                return null;

            GameObject[] children = new GameObject[obj.childCount];

            for (int i = 0; i < obj.childCount; i++) children[i] = obj.GetChild(i).gameObject;

            return children;
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
            for (int i = transform.childCount - 1; i >= 0; i--) DestroyImmediate(transform.GetChild(i).gameObject);
        }

        #endregion
    }
}