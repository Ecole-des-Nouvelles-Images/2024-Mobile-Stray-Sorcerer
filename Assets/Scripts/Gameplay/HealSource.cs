using AI;
using Player;
using UnityEngine;

namespace Gameplay
{
    public class HealSource : MonoBehaviour
    {
        [SerializeField] private int _healValue = 1;
        [SerializeField] private PlayerDetector _playerDetector;

        private Rigidbody _rb;
        private bool _chasePlayer;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            _playerDetector.OnPlayerDetected += StartChase;
        }

        private void OnDisable()
        {
            _playerDetector.OnPlayerDetected -= StartChase;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player") && Character.Instance.HP < Character.Instance.MaxHP)
            {
                Character.Instance.TakeHeal(_healValue);
                Destroy(transform.parent.gameObject);
            }
        }
        private void Update()
        {
            if (_rb.velocity != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(_rb.velocity, Vector3.up);
                _rb.MoveRotation(targetRotation);
            }

            if (Character.Instance)
            {
                if (_chasePlayer && Character.Instance.HP < Character.Instance.MaxHP)
                {
                    _rb.velocity = (Character.Instance.transform.position - transform.position).normalized * (100 * Time.fixedDeltaTime);
                }
                else
                {
                    _rb.velocity = Vector3.zero;
                }
            }

        }

        private void StartChase(bool chasePlayer)
        {
            if(chasePlayer && Character.Instance.HP < Character.Instance.MaxHP)
                _chasePlayer = true;
        }
    }
}