using UnityEngine;

namespace MazeGenerator
{
    public class Cell: MonoBehaviour
    {
        public Vector2 Position { get; set; }

        [SerializeField] private GameObject _wallTop;
        [SerializeField] private GameObject _wallRight;
        [SerializeField] private GameObject _wallBottom;
        [SerializeField] private GameObject _wallLeft;
    }
}