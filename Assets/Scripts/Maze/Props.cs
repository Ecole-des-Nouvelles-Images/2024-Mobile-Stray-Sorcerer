using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Maze
{
    public class Props: MonoBehaviour
    {
        [Serializable] [Flags]
        public enum Type
        {
            Barrel = 1,
            Crates = 2,
            Banner = 4,
            Rubble = 8,
            // BrazierColumn = 16,
        }

        [SerializeField] private Type _propType;
        [SerializeField] private bool _useCustomProbabilities = false;
        [SerializeField] private float _globalProbabilityPerSlot = 0.5f;

        [SerializeField] private List<GameObject> _barrelPrefabs = new ();
        [SerializeField] [Range(0,1)] private float _barrelProbability = 0.5f;
        [SerializeField] private List<GameObject> _cratesPrefabs = new ();
        [SerializeField] [Range(0,1)] private float _cratesProbability = 0.5f;
        [SerializeField] private List<GameObject> _bannerPrefabs = new ();
        [SerializeField] [Range(0,1)] private float _bannerProbability = 0.5f;
        [SerializeField] private List<GameObject> _rubblePrefabs = new ();
        [SerializeField] [Range(0,1)] private float _rubbleProbability = 0.5f;
        // [SerializeField] private List<GameObject> _brazierColumnPrefabs = new ();
        // [SerializeField] [Range(0,1)] private float _brazierColumnProbability = 0.5f;

        public Dictionary<string, List<GameObject>> Prefabs = new();

        public Type PropType => _propType;

        public List<GameObject> BarrelPrefabs => _barrelPrefabs;
        public List<GameObject> CratesPrefabs => _cratesPrefabs;
        public List<GameObject> BannerPrefabs => _bannerPrefabs;
        public List<GameObject> RubblePrefabs => _rubblePrefabs;
        // public List<GameObject> BrazierColumnPrefabs => _brazierColumnPrefabs;

        public static IEnumerable<Enum> GetFlags(Enum e)
        {
            return Enum.GetValues(e.GetType()).Cast<Enum>().Where(v => !Equals((int)(object)v, 0) && e.HasFlag(v));
        }
    }
}