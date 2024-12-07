using System;
using UnityEngine;

namespace AI
{
    public class PlayerDetector : MonoBehaviour
    {
        
        public bool DetectObject;
        
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player") && DetectObject == false)
            {
                DetectObject = true;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if(other.CompareTag("Player") && DetectObject == false)
            {
                DetectObject = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.CompareTag("Player") && DetectObject)
            {
                DetectObject = false;
            }
        }
    }
}