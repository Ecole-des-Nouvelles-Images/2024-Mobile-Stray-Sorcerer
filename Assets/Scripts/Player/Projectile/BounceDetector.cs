using UnityEngine;

namespace Player.Projectile
{
    public class BounceDetector : MonoBehaviour
    {
        public bool IsBounceCollideActive { get; private set; }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag("Wall") && IsBounceCollideActive == false)
                IsBounceCollideActive = true;
            if (other.transform.CompareTag("Enemy") || other.transform.CompareTag("Player") || other.transform.CompareTag("Projectile"))
                IsBounceCollideActive = false;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.CompareTag("Wall") && IsBounceCollideActive)
                IsBounceCollideActive = false;
            if (other.transform.CompareTag("Enemy") || other.transform.CompareTag("Player") || other.transform.CompareTag("Projectile"))
                IsBounceCollideActive = true;
        }
    }
}