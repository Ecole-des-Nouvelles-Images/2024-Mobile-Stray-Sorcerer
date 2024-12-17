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

        private void Update()
        {
            if (_rb.velocity != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(_rb.velocity, Vector3.up);
                _rb.MoveRotation(targetRotation);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Character.Instance.TakeDamage(Damage);
                Destroy(gameObject);
            }

            if (other.CompareTag("Wall")) Destroy(gameObject);
        }

        public void ShootToDestination(Transform destination, int damage, int power)
        {
            Damage = damage;
            _rb.AddForce((destination.position - transform.position).normalized * power + Vector3.up, ForceMode.Impulse);
        }
        //(destination.position - transform.position).normalized
    }
}