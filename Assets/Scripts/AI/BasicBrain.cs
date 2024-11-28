using UnityEngine;
using UnityEngine.AI;

namespace AI
{
   public class BasicBrain : MonoBehaviour
   {
      public GameObject MyTarget;
      private NavMeshAgent _myNavMeshAgent;

      private void Start()
      {
         _myNavMeshAgent = transform.GetComponent<NavMeshAgent>();
      }

      private void Update()
      {
         if (MyTarget != null)
         {
            _myNavMeshAgent.SetDestination(MyTarget.transform.position);
         }
      }
   }
}
