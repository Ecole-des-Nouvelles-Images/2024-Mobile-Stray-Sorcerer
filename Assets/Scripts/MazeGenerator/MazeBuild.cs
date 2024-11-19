using System;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

namespace MazeGenerator
{
    public class MazeBuild : MonoBehaviour
    {
        [SerializeField] private GameObject _cellPrefab;
        [SerializeField] private int _scale = 1;

        private void Start()
        {
            BuildMaze();
        }

        [ContextMenu("Build Maze")]
        public void BuildMaze()
        {
            ClearMaze();

            for (int y = 0; y < _scale; y++)
            {
                for (int x = 0; x < _scale; x++)
                {
                    Instantiate(_cellPrefab, new Vector3(x * 20, 0, y * 20), Quaternion.identity, transform);
                }
            }
        }

        [ContextMenu("Clear Maze")]
        private void ClearMaze()
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }
    }
}
