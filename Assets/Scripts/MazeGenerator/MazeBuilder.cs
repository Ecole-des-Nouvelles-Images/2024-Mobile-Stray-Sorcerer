using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

using Random = System.Random;

namespace MazeGenerator
{
    public class MazeBuilder : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _cellPrefab;

        [Header("Settings")]
        [SerializeField] private string _seed;
        [SerializeField] private int _scale = 1;

        public static Cell[,] Grid;

        public int Seed => HashSeed(_seed);

        private Stack<Cell> _buildStack = new();

        // --------------------- //

        private void Start()
        {
            Grid = new Cell[_scale, _scale];
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

        public static Cell GetCell(int x, int y)
        {
            if (x >= Grid.Length || x < 0 ||
                y >= Grid.Length || y < 0)
                return null;

            return Grid[x, y];
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