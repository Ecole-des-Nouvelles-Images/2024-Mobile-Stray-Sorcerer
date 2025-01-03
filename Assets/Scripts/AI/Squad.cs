using System;
using System.Collections.Generic;
using AI.Monsters;
using Player;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AI
{
    public class Squad : MonoBehaviour
    {
        [Range(1, 100), SerializeField] private int _procRatio = 40;
        [SerializeField] private List<GameObject> _monsterPrefabs;
        [SerializeField] private Transform[] _markerList;
        [SerializeField] private Transform _raycastOrigin;
        [SerializeField] private GameObject _speedBoostAreaPrefab;
        
        private bool _isTriggered;
        private bool _isChaseTime;
        private List<GameObject> _spawnedMonsters;

        private void Start()
        {
            SquadGeneration();
        }

        private void Update()
        {
            if (_isTriggered && AllMonstersDied())
            {
                int dice = Random.Range(1, 100);
                if (dice <= _procRatio)
                    Instantiate(_speedBoostAreaPrefab, transform.position, quaternion.identity);
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !_isTriggered)
            {
                _isTriggered = true;
                for (int i = 0; i < _markerList.Length; i++)
                {
                    _markerList[i].GetChild(0).gameObject.SetActive(true);
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player") && !_isChaseTime && Character.Instance)
            {
                PlayerDirectView();
            }
        }

        private void SquadGeneration()
        {
            for (int i = 0; i < _markerList.Length; i++)
            {
                var monster= Instantiate(GetRandomMonster(), _markerList[i].position, _markerList[i].rotation,_markerList[i]);
                monster.SetActive(true);
            }
        }

        private GameObject GetRandomMonster()
        {
            return _monsterPrefabs[Random.Range(0, _monsterPrefabs.Count)];
        }

        private bool AllMonstersDied()
        {
            for (int i = 0; i < _markerList.Length; i++)
            {
                if(_markerList[i].childCount > 0)
                    return false;
            }
            return true;
        }

        private void PlayerDirectView()
        {
            RaycastHit hit;
            LayerMask layerMask = LayerMask.GetMask("Player","Wall");
            
            if (Physics.Raycast(_raycastOrigin.position,
                    Character.Instance.EnemyRaycastTarget.position - _raycastOrigin.position, out hit,
                    Mathf.Infinity, layerMask, QueryTriggerInteraction.Ignore))
            {
                if (hit.collider.gameObject == Character.Instance.gameObject)
                {
                    _isChaseTime = true;
                    for (int i = 0; i < _markerList.Length; i++)
                    {
                        _markerList[i].GetChild(0).gameObject.GetComponent<Monster>()
                            .DefineTarget(Character.Instance.gameObject);
                    }
                }
            }
        }
    }
}
