using System;
using UnityEngine;

namespace AI
{
    public class PlayerDetector : MonoBehaviour
    {
        [SerializeField] private BasicBrain _brain;

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
                _brain.PlayerInRange = true;
        }

        private void OnTriggerStay(Collider other)
        {
            if(other.CompareTag("Player") && _brain.PlayerInRange == false)
                _brain.PlayerInRange = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.CompareTag("Player"))
                _brain.PlayerInRange = false;
        }
    }
}