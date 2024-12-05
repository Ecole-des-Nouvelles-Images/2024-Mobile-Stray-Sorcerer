using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AI
{
    public class Squad : MonoBehaviour
    {
        
        [SerializeField] private List<GameObject> _monsterPrefabs;
        [SerializeField] private Transform[] _markerList;
        [SerializeField][Range(0,9)] private int _maxMonsterCount;
        
        private int _monsterCount;
        private GameObject[,] _monstersDataPositions;

        private void Awake()
        {
            _monstersDataPositions = new GameObject [_markerList.Length, _markerList.Length];
            if (_maxMonsterCount == 0)
                _monsterCount = Random.Range(1, _markerList.Length);
            else
                _monsterCount = _maxMonsterCount;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                //activation des bots
            }
        }

        private void SquadGeneration()
        {
            for (int i = 0; i < _maxMonsterCount; i++)
            {
                int randomX = Random.Range(0, _markerList.Length);
                int randomY = Random.Range(0, _markerList.Length);
                if (_monstersDataPositions[randomX, randomY] == null)
                {
                    _monstersDataPositions[randomX, randomY] = GetRandomMonster();
                }
            }
            
        }

        private GameObject GetRandomMonster()
        {
            return _monsterPrefabs[Random.Range(0, _monsterPrefabs.Count)];
        }
    }
}
