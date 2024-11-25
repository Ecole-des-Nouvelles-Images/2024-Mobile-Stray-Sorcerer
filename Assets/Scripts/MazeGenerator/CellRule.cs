using System;
using UnityEngine;

namespace MazeGenerator
{
    [Serializable]
    public struct CellRule
    {
        public bool Top;
        public bool Right;
        public bool Bottom;
        public bool Left;
        public GameObject Prefab;
    }
}