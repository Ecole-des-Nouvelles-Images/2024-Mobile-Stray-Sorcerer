using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using Utils;
using Random = System.Random;

namespace MazeGenerator
{
    public class MazeBuilder : SingletonMonoBehaviour<MazeBuilder>
    {
        [Header("References")]
        [SerializeField] private GameObject _cellInitialPrefab;
        [SerializeField] private List<CellRule> _cellRules;

        [Header("Settings")]
        [SerializeField] private string _seed;
        [SerializeField] private int _scale = 1;

        public int Seed => HashSeed(_seed);

        private Maze _maze;

        // --------------------- //

        private void Start()
        {
            _maze = new Maze(_scale);
            _maze.Initialize(_cellInitialPrefab);
            Build();
        }

        [ContextMenu("Build Maze")]
        public void Build()
        {
            Clear();

            for (int y = 0; y < _maze.Scale; y++)
            {
                for (int x = 0; x < _maze.Scale; x++)
                {
                    Cell cell = _maze.GetCell(x, y);

                    if (cell != null)
                        Instantiate(cell.Prefab, new Vector3(x * 20, 0, y * 20), Quaternion.identity, transform);
                    else
                        throw new Exception("Grid's cell should not be null.");
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
    }
}