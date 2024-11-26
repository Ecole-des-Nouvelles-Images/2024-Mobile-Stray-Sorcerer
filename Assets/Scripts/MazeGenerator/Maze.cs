using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Random = System.Random;

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

        public void Generate()
        {
            Initialize();
            DepthFirstSearchPass();
        }

        private void Initialize()
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

        private void DepthFirstSearchPass()
        {
            Stack<Cell> buildStack = new Stack<Cell>();
            Random generator = new Random(MazeBuilder.Instance.Seed);

            Cell origin = Grid[0, 0];
            origin.Visited = true;
            buildStack.Push(origin);

            while (buildStack.Count > 0)
            {
                Cell current = buildStack.Peek();
                List<Cell> neighbors = GetUnvisitedNeighbors(current);

                if (neighbors.Count == 0)
                {
                    buildStack.Pop();
                }
                else
                {
                    Cell neighbor = neighbors[generator.Next(0, neighbors.Count)];
                    RemoveWall(current, neighbor);
                    neighbor.Visited = true;
                    buildStack.Push(neighbor);
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

        private List<Cell> GetUnvisitedNeighbors(Cell cell)
        {
            List<Cell> neighbors = new List<Cell>();

            if (cell.Position.y > 0)
            {
                Cell top = GetCell(cell.Position.x, cell.Position.y - 1);
                if (!top.Visited)
                    neighbors.Add(top);
            }

            if (cell.Position.x < Scale - 1)
            {
                Cell right = GetCell(cell.Position.x + 1, cell.Position.y);
                if (!right.Visited)
                    neighbors.Add(right);
            }

            if (cell.Position.y < Scale - 1)
            {
                Cell bottom = GetCell(cell.Position.x, cell.Position.y + 1);
                if (!bottom.Visited)
                    neighbors.Add(bottom);
            }

            if (cell.Position.x > 0)
            {
                Cell left = GetCell(cell.Position.x - 1, cell.Position.y);
                if (!left.Visited)
                    neighbors.Add(left);
            }

            return neighbors;
        }

        private void RemoveWall(Cell current, Cell destination)
        {
            Vector2Int currentPos = current.Position;
            Vector2Int destinationPos = destination.Position;

            if (destinationPos.y == currentPos.y + 1) // Destination is above
            {
                current.WallTop = false;
                destination.WallBottom = false;
            }
            else if (destinationPos.x == currentPos.x + 1) // Destination is on the right
            {
                current.WallRight = false;
                destination.WallLeft = false;
            }
            else if (destinationPos.y == currentPos.y - 1) // Destination is below
            {
                current.WallBottom = false;
                destination.WallTop = false;
            }
            else if (destinationPos.x == currentPos.x - 1) // Destination is on the left
            {
                current.WallLeft = false;
                destination.WallRight = false;
            }
        }
    }
}