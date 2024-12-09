using System;
using UnityEngine;

namespace Maze
{
    public class Props
    {
        [Flags]
        public enum Type
        {
            None = 0,
            Barrel = 1,
            BrazierColumn = 2,
            Crates = 4,
            Flag = 8,
            Rubble = 16,
        }

        [field: SerializeField] public Type PropType { get; private set; }
        [SerializeField] private float _barrelProbability = 0.5f;
        [SerializeField] private float _brazierColumnProbability = 0.5f;
        [SerializeField] private float _cratesProbability = 0.5f;
        [SerializeField] private float _flagProbability = 0.5f;
        [SerializeField] private float _rubbleProbability = 0.5f;
    }
}