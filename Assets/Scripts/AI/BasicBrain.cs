using System;
using Player;
using UnityEngine;
using UnityEngine.AI;
using Utils;

namespace AI
{
   public abstract class BasicBrain : MonoBehaviour
   {
      public bool PlayerInRange;

      [SerializeField] private GameObject _playerDetector;
      [SerializeField] private int _hpMax;
      [SerializeField] private int _hpGrowingFactor;
      [SerializeField] private int _damage;
      [SerializeField] private int _damageGrowingFactor;
      [SerializeField] private int _speed;
      [SerializeField] private int _acceleration;
      
      private NavMeshAgent _myNavMeshAgent;
      private GameObject _myTarget;

      private void Awake()
      {
         _myNavMeshAgent = transform.GetComponent<NavMeshAgent>();
         _myTarget = Character.Instance.gameObject;
      }

      private void Start()
      {
         
      }

      private void Update()
      {
         if (_myTarget != null && !PlayerInRange)
         {
            _myNavMeshAgent.SetDestination(_myTarget.transform.position);
         }

         if (PlayerInRange )
         {
            _myNavMeshAgent.SetDestination(transform.position);
             Attack();
         }
      }

      protected abstract void Attack();

   }
}
