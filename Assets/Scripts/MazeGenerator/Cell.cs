using System;
using UnityEngine;

namespace MazeGenerator
{
    public class Cell: MonoBehaviour
    {
        public Vector2Int Position { get; set; }

        [SerializeField] private GameObject _wallTop;
        [SerializeField] private GameObject _wallRight;
        [SerializeField] private GameObject _wallBottom;
        [SerializeField] private GameObject _wallLeft;

        public bool IsVisited { get; set; } = false;
    }
}