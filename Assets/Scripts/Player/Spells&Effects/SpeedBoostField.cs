using UnityEngine;

namespace Player.Spells_Effects
{
    public class SpeedBoostField : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !Character.Instance.IsBoosted) Character.OnSpeedBoost.Invoke(true);
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player") && !Character.Instance.IsBoosted) Character.OnSpeedBoost.Invoke(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player")) Character.OnSpeedBoost.Invoke(false);
        }
    }
}