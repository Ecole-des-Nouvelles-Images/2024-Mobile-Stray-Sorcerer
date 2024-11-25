using UnityEngine;

namespace Christopher.C.Scripts.Player.Projectile
{
    public class HitEffects : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            if (!other.transform.CompareTag("Player"))
            {
                Destroy(gameObject);
            }
        }

    }
}
