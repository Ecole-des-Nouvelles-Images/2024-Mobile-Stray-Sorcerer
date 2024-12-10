using System;
using Player;
using UnityEngine;

namespace AI
{
    public class CreatureProjectile : MonoBehaviour
    {
        public int Damage;
        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

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

        public void ShootToDestination(Transform destination, int damage, int power)
        {
            Damage = damage;
            _rb.AddForce(destination.position - transform.position* power, ForceMode.Impulse);
        }
    }
}
