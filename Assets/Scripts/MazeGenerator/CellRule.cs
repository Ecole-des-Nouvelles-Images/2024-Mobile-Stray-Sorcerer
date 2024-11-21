using System;
using UnityEngine;

namespace MazeGenerator
{
    [System.Serializable]
    public struct CellRule
    {
        public bool Top;
        public bool Right;
        public bool Bottom;
        public bool Left;
        public GameObject Prefab;
    }
}