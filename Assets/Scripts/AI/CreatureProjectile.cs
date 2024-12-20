using Player;
using UnityEngine;

namespace AI
{
    public class CreatureProjectile : MonoBehaviour
    {
        private int _damage;
        private GameObject _impactFX;
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
                Character.Instance.TakeDamage(_damage);
                CreateImpact();
                Destroy(gameObject);
            }

            if (other.CompareTag("Wall"))
            {
                CreateImpact();
                Destroy(gameObject);
            }
        }

        private void CreateImpact()
        {
            GameObject impact = Instantiate(_impactFX, transform.position, Quaternion.identity);
            Destroy(impact,1);
        }

        public void ShootToDestination(Transform destination, int damage, int power, GameObject impact)
        {
            _impactFX = impact;
            _damage = damage;
            _rb.AddForce((destination.position - transform.position).normalized * power + Vector3.up, ForceMode.Impulse);
        }
    }
}