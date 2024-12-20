using System;
using UnityEngine;

namespace AI
{
    public class PlayerDetector : MonoBehaviour
    {
        public Action<bool> OnPlayerDetected;
        private bool _detectObject;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && _detectObject == false)
            {
                _detectObject = true;
                OnPlayerDetected?.Invoke(true);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player") && _detectObject == false)
            {
                _detectObject = true;
                OnPlayerDetected?.Invoke(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") && _detectObject)
            {
                _detectObject = false;
                OnPlayerDetected?.Invoke(false);
            }
        }
    }
}