using Player;
using UnityEngine;
using UnityEngine.AI;
using Utils;

namespace AI
{
   public class BasicBrain : MonoBehaviour
   {
      
      
      public bool PlayerInRange;

      [SerializeField] private GameObject _meleeDetector;
      [SerializeField] private GameObject _rangeDetector;
      [SerializeField] private bool _haveRangeAttack;
      private NavMeshAgent _myNavMeshAgent;
      private GameObject _myTarget;
      private Rigidbody _rb;
      
      
      private void Start()
      {
         if (_haveRangeAttack)
         {
            _meleeDetector.SetActive(false);
         }
         else
         {
            _rangeDetector.SetActive(false);
         }
         _myNavMeshAgent = transform.GetComponent<NavMeshAgent>();
         _myTarget = Character.Instance.gameObject;
         _rb = transform.GetComponent<Rigidbody>();
      }

      private void Update()
      {
         if (_myTarget != null && !PlayerInRange)
         {
            _myNavMeshAgent.SetDestination(_myTarget.transform.position);
         }

         if (PlayerInRange && _haveRangeAttack)
         {
            _myNavMeshAgent.SetDestination(transform.position);
             //do range attack
             Debug.Log("Range attack!");
         }
         if (PlayerInRange && !_haveRangeAttack)
         {
            _myNavMeshAgent.SetDestination(transform.position);
            //do melee attack
            Debug.Log("Melee attack!");
         }
      }
      
   }
}
