using UnityEngine;

namespace MazeGenerator
{
    public class Maze
    {
        public Cell[,] Grid { get ; private set;}

        public int Scale { get; private set; }

        public Maze(int scale)
        {
            Scale = scale;
        }

        public void Initialize(GameObject initialPrefab)
        {
            Grid = new Cell[Scale, Scale];

            for (int y = 0; y < Scale; y++)
            {
                for (int x = 0; x < Scale; x++)
                {
                    Grid[x, y] = new Cell(x, y);
                }
            }
        }

        public Cell GetCell(int x, int y)
        {
            if (x >= Grid.Length || x < 0 ||
                y >= Grid.Length || y < 0)
                return null;

            return Grid[x, y];
        }
    }
}