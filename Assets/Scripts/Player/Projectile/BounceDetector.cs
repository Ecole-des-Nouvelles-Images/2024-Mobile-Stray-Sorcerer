using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Player.Projectile
{
    public class BounceDetector : MonoBehaviour
    {
        public bool IsBounceColideActive { get; private set; }
        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag("Wall") && IsBounceColideActive == false)
                IsBounceColideActive = true;
            if (other.transform.CompareTag("Enemy") || other.transform.CompareTag("Player") || other.transform.CompareTag("Projectile"))
                IsBounceColideActive = false;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.CompareTag("Wall") && IsBounceColideActive )
                IsBounceColideActive = false;
            if (other.transform.CompareTag("Enemy") || other.transform.CompareTag("Player") || other.transform.CompareTag("Projectile"))
                IsBounceColideActive = true;
        }
    }
}