using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using Utils;

namespace MazeGenerator
{
    public class MazeBuilder : SingletonMonoBehaviour<MazeBuilder>
    {
        [Header("References")]
        [SerializeField] private GameObject _cellInitialPrefab;
        [SerializeField] private List<CellRule> _cellRules;

        [Header("Settings")]
        [SerializeField] private string _seedPhrase;
        [SerializeField] private int _scale = 1;

        private string _seed;
        private int _hashSeed;

        public int Seed {
            get
            {
                if (_seed != _seedPhrase) {
                    _seed = _seedPhrase;
                    _hashSeed = HashSeed(_seed);
                }

                return _hashSeed;
            }
        }

        private Maze _maze;

        // --------------------- //

        private void Start()
        {
            Build();
        }

        [ContextMenu("Build Maze")]
        public void Build()
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
                        Instantiate(cell.Prefab, new Vector3(x * 20, 0, y * 20), Quaternion.identity, transform);
                    else
                        throw new Exception("Error: invalid grid, null cell found.");
                }
            }
        }

        [ContextMenu("Clear Maze")]
        private void Clear()
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
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

        public GameObject GetCellPrefab(Cell cell)
        {
            foreach (CellRule rule in _cellRules)
            {
                if (rule.Top == cell.WallTop && rule.Right == cell.WallRight && rule.Bottom == cell.WallBottom && rule.Left == cell.WallLeft)
                    return rule.Prefab;
            }

            return _cellInitialPrefab;
        }

        #endregion
    }
}