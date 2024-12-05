using Player;
using UnityEngine;

namespace AI
{
    public class CreatureProjectile : MonoBehaviour
    {
        public int Damage;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Character.Instance.TakeDamage(Damage);
                Destroy(gameObject);
            }
            if (other.CompareTag("Wall"))
            {
                Destroy(gameObject);
            }
        }
    }
}
