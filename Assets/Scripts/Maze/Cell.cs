using UnityEngine;

namespace Maze
{
    public class Cell
    {
        public const int CellSize = 20;

        public Vector2Int Position { get; private set; }
        public GameObject Prefab => MazeBuilder.Instance.GetCellPrefab(this);

        public bool WallTop { get; set; } = true;
        public bool WallRight { get; set; } = true;
        public bool WallBottom { get; set; } = true;
        public bool WallLeft { get; set; } = true;

        public bool Visited { get; set; }

        public Cell(int x, int y)
        {
            Position = new Vector2Int(x, y);
        }
    }
}