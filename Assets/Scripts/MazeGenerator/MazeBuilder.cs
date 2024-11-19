using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace MazeGenerator
{
    public class MazeBuilder : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _cellPrefab;

        [Header("Settings")]
        [SerializeField] private string _seed;
        [SerializeField] private int _scale = 1;

        public int Seed => HashSeed(_seed);
        private Cell[,] _grid;
        private Stack<Cell> _buildStack = new();

        private void Start()
        {
            _grid = new Cell[_scale, _scale];
        }

        [ContextMenu("Build Maze")]
        public void Build()
        {
            Clear();

            for (int y = 0; y < _scale; y++)
            {
                for (int x = 0; x < _scale; x++)
                {
                    Cell cell = Instantiate(_cellPrefab, new Vector3(x * 20, 0, y * 20), Quaternion.identity, transform).GetComponent<Cell>();
                    cell.Position = new Vector2Int(x, y);
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

        [ContextMenu("Generate Maze")]
        public void Generate()
        {
            Build();
            Cell origin = GetCell(0, 0);

            _buildStack.Push(origin);

            while (_buildStack.Count > 0)
            {
                Cell current = _buildStack.Pop();


            }
        }

        // --------------------- //

        private Cell GetCell(Vector2Int position)
        {
            return _grid[position.x, position.y];
        }

        private Cell GetCell(int x, int y)
        {
            return _grid[x, y];
        }

        private int HashSeed(string seed)
        {
            using (SHA1 sha1 = new SHA1Managed())
            {
                byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(seed));
                return BitConverter.ToInt32(hash, 0);
            }
        }
    }
}