using System;
using System.Collections.Generic;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AI
{
    public class Squad : MonoBehaviour
    {
        
        [SerializeField] private List<GameObject> _monsterPrefabs;
        [SerializeField] private Transform[] _markerList;
        [SerializeField][Range(0,9)] private int _maxMonsterCount;
        [SerializeField] private Transform _raycastOrigin;
        
        private int _monsterCount;
        private bool _isTriggered;
        private bool _isChaseTime;
        private List<GameObject> _spawnedMonsters;

        private void Awake()
        {
            if (_maxMonsterCount == 0)
                _monsterCount = Random.Range(1, _markerList.Length);
            else
                _monsterCount = _maxMonsterCount;
        }

        private void Start()
        {
            SquadGeneration();
        }

        private void Update()
        {
            if(!_isChaseTime)
                PlayerDirectView();
            if (_isTriggered && AllMonstersDied())
            {
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

        private void SquadGeneration()
        {
            for (int i = 0; i < _markerList.Length; i++)
            {
                var monster= Instantiate(GetRandomMonster(), _markerList[i].position, _markerList[i].rotation,_markerList[i]);
                monster.SetActive(false);
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
                if(_markerList[i].GetChild(0).gameObject.activeSelf)
                    return false;
            }
            return true;
        }

        private void PlayerDirectView()
        {
            RaycastHit hit;
            LayerMask layerMask = LayerMask.GetMask("Player","Wall");
            if (Physics.Raycast(_raycastOrigin.position,
                    Character.Instance.EnnemyRaycastTarget.position - _raycastOrigin.position, out hit,
                    Mathf.Infinity, layerMask, QueryTriggerInteraction.Ignore))
            {
                Debug.DrawRay(_raycastOrigin.position, 
                    (Character.Instance.EnnemyRaycastTarget.position - _raycastOrigin.position) * hit.distance, Color.green);
                
                if (hit.collider.gameObject == Character.Instance.gameObject)
                {
                    Debug.Log("player track begin");
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
