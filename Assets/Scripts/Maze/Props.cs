﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Maze
{
    public class Props : MonoBehaviour
    {
        [Serializable]
        [Flags]
        public enum Type
        {
            Barrel = 1,
            Crates = 2,
            Banner = 4,
            Rubble = 8,
            SpiderWebs = 16,
            Rug = 32,
            // BrazierColumn = 64,
        }

        [SerializeField] private Type _propType;
        [SerializeField] private bool _useCustomProbabilities;

        [SerializeField] private List<GameObject> _barrelPrefabs = new();
        [SerializeField] [Range(0, 1)] private float _barrelProbability = 0.5f;
        [SerializeField] private List<GameObject> _cratesPrefabs = new();
        [SerializeField] [Range(0, 1)] private float _cratesProbability = 0.5f;
        [SerializeField] private List<GameObject> _bannerPrefabs = new();
        [SerializeField] [Range(0, 1)] private float _bannerProbability = 0.5f;
        [SerializeField] private List<GameObject> _rubblePrefabs = new();
        [SerializeField] [Range(0, 1)] private float _rubbleProbability = 0.5f;
        [SerializeField] private List<GameObject> _spiderWebsPrefabs = new();
        [SerializeField] [Range(0, 1)] private float _spiderWebsProbability = 0.5f;
        [SerializeField] private List<GameObject> _rugPrefabs = new();
        [SerializeField] [Range(0, 1)] private float _rugProbability = 0.5f;
        // [SerializeField] private List<GameObject> _brazierColumnPrefabs = new ();
        // [SerializeField] [Range(0,1)] private float _brazierColumnProbability = 0.5f;

        public const float GLOBAL_PROBABILITY_PER_SLOT = 0.5f;

        public Dictionary<string, (List<GameObject> list, float probability)> Prefabs = new();

        public Type PropType => _propType;
        public bool UseCustomProbabilities => _useCustomProbabilities;

        public List<GameObject> BarrelPrefabs => _barrelPrefabs;
        public float BarrelProbability => _barrelProbability;
        public List<GameObject> CratesPrefabs => _cratesPrefabs;
        public float CratesProbability => _cratesProbability;
        public List<GameObject> BannerPrefabs => _bannerPrefabs;
        public float BannerProbability => _bannerProbability;
        public List<GameObject> RubblePrefabs => _rubblePrefabs;
        public float RubbleProbability => _rubbleProbability;
        public List<GameObject> SpiderWebsPrefabs => _spiderWebsPrefabs;
        public float SpiderWebsProbability => _spiderWebsProbability;
        public List<GameObject> RugPrefabs => _rugPrefabs;
        public float RugProbability => _rugProbability;
        // public List<GameObject> BrazierColumnPrefabs => _brazierColumnPrefabs;
        // public float BrazierColumnProbability => _brazierColumnProbability;

        public static IEnumerable<Enum> GetFlags(Enum e)
        {
            return Enum.GetValues(e.GetType()).Cast<Enum>().Where(v => !Equals((int)(object)v, 0) && e.HasFlag(v));
        }

        public void UpdateDictionary()
        {
            Props props = this;

            props.Prefabs = new Dictionary<string, (List<GameObject> list, float probability)>
            {
                { "Barrel",     (props.BarrelPrefabs, props.BarrelProbability) },
                { "Crates",     (props.CratesPrefabs, props.CratesProbability) },
                { "Banner",     (props.BannerPrefabs, props.BannerProbability) },
                { "Rubble",     (props.RubblePrefabs, props.RubbleProbability) },
                { "SpiderWebs", (props.SpiderWebsPrefabs, props.SpiderWebsProbability) },
                { "Rug",        (props.RugPrefabs, props.RugProbability) }
                // { "BrazierColumn", props.BrazierColumnPrefabs }
            };
        }
    }
}