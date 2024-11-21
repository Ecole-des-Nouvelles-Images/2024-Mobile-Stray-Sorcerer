using UnityEngine;

namespace MazeGenerator
{
    public class Cell
    {
        public const int CellSize = 20;

        public Vector2Int Position { get; private set; }
        public GameObject Prefab => MazeBuilder.Instance.GetCellPrefab(this);

        public bool WallTop { get; set; }
        public bool WallRight { get; set; }
        public bool WallBottom { get; set; }
        public bool WallLeft { get; set; }

        public Cell(int x, int y)
        {
            Position = new Vector2Int(x, y);
        }
    }
}