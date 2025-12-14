using System.Collections.Generic;
using AI.Monsters;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AI
{
    public class SquadWave : MonoBehaviour
    {
        /*
         * Valeur du timer aléatoire entre x et y
         */
        [SerializeField] private Vector2 _timeToRespawn;
        [SerializeField] private List<GameObject> _monsterPrefabs;
        [SerializeField] private Transform[] _markerList;
        
        private bool _isSpawnEnable;
        /*
         * temps actuel pour un prochain spawn
         */
        private float _currentTimerValue;
        /*
         * valeur actuel du timer avant le spawn
         */
        private float _currentTime;
        private List<GameObject> _spawnedMonsters;

        private void Start()
        {
            if (_timeToRespawn.x > _timeToRespawn.y) _timeToRespawn.y = _timeToRespawn.x;
            _currentTimerValue = SelectedTimeValue();
            SquadGeneration();
        }
        private void Update()
        {
            if ( Character.Instance.gameObject)
            {
                if (_currentTime < _currentTimerValue) _currentTime += Time.deltaTime;
                else
                {
                    SquadGeneration();
                    _currentTime = 0;
                    _currentTimerValue = SelectedTimeValue();
                }
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && _isSpawnEnable)
            {
                _isSpawnEnable = false;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") && !_isSpawnEnable)
            {
                _isSpawnEnable = true;
            }
        }
        private float SelectedTimeValue()
        {
            return Random.Range(_timeToRespawn.x, _timeToRespawn.y);
        }
        private void SquadGeneration()
        {
            if (_markerList.Length != 0)
            {
                for (int i = 0; i < Random.Range(1,_markerList.Length); i++)
                {
                    var monster= Instantiate(GetRandomMonster(), _markerList[i].position, _markerList[i].rotation,_markerList[i]);
                    monster.SetActive(true);
                    Debug.Log("Chasse!");
                    _markerList[i].GetChild(0)?.gameObject.GetComponent<Monster>().DefineTarget(Character.Instance.gameObject);
                }
            } 
        }
        private GameObject GetRandomMonster()
        {
            return _monsterPrefabs[Random.Range(0, _monsterPrefabs.Count)];
        }
    }
}
