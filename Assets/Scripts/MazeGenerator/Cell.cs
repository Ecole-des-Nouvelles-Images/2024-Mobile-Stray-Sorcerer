using System;
using System.Collections.Generic;
using UnityEngine;

namespace MazeGenerator
{
    public class Cell: MonoBehaviour
    {
        public Vector2Int Position { get; set; }

        public GameObject WallTop;
        public GameObject WallRight;
        public GameObject WallBottom;
        public GameObject WallLeft;

        public bool IsVisited { get; set; } = false;
        public List<Cell> Neighbors { get; set; }
        public bool HasUnvisitedNeighbors => Neighbors.Exists(neighbor => !neighbor.IsVisited);

        private void Awake()
        {
            Neighbors = new();
        }

    }
}