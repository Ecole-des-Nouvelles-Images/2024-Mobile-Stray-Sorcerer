using AI.Monsters;
using Player.Spells_Effects;
using UnityEngine;

namespace Player.Projectile
{
    public class PlayerProjectile : MonoBehaviour
    {
        [SerializeField] private BounceDetector _bounceDetector;
        private GameObject _impactPrefab;
        private Rigidbody _rb;
        private Collider _myCollider;
        private Spell _mySpell;
        private string _name;
        private GameObject _projectilePrefab;
        private int _damage;
        private bool _pierce;
        private int _pierceValue;
        private bool _bounce;
        private int _bounceValue;
        private bool _areaInvoker;
        private GameObject _areaPrefab;

        private void Awake()
        {
            _rb = transform.GetComponent<Rigidbody>();
            _myCollider = transform.GetComponent<Collider>();
            _mySpell = Character.Instance.CurrentSpell;
            _name = _mySpell.Name;
            _damage = _mySpell.Damage + (int)Character.Instance.SpellPower;
            _pierce = _mySpell.Pierce;
            _pierceValue = _mySpell.PierceValue;
            _bounce = _mySpell.Bounce;
            _bounceValue = _mySpell.BounceValue;
            _areaInvoker = _mySpell.AreaInvoker;
            _areaPrefab = _mySpell.ZonePrefab;
        }

        private void Start()
        {
            if (_bounce)
                _bounceDetector.gameObject.SetActive(true);
            else
                _bounceDetector.gameObject.SetActive(false);
            _myCollider.isTrigger = true;
        }

        private void Update()
        {
            if (_rb.velocity != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(_rb.velocity, Vector3.up);
                _rb.MoveRotation(targetRotation);
            }

            if (_bounceDetector.IsBounceColideActive && _myCollider.isTrigger && _bounce)
                _myCollider.isTrigger = false;
            if (_bounceDetector.IsBounceColideActive == false && _myCollider.isTrigger == false && _bounce)
                _myCollider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag("Wall") && _pierce) Destroy(gameObject);
            if (other.transform.CompareTag("Enemy"))
            {
                _rb.constraints = RigidbodyConstraints.FreezePositionY;
                CastArea(other.transform);
                other.transform.GetComponent<Monster>().TakeDamage(_damage);
                if (!_pierce && !_bounce)
                    Destroy(gameObject);
                if (_pierce && _pierceValue <= 0)
                    Destroy(gameObject);
                else if (_pierce) _pierceValue--;
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.CompareTag("Player"))
            {
                if (_bounce) _myCollider.isTrigger = true;
                return;
            }

            if (other.transform.CompareTag("Enemy"))
            {
                _rb.constraints = RigidbodyConstraints.FreezePositionY;
                CastArea(other.transform);
                other.gameObject.GetComponent<Monster>().TakeDamage(_damage);
                Destroy(gameObject);
            }

            if (other.transform.CompareTag("Wall"))
            {
                if (_bounce && _bounceValue <= 0)
                    Destroy(gameObject);
                else
                    _bounceValue--;
                if (_bounce == false) Destroy(gameObject);
            }
        }

        private void OnCollisionExit(Collision other)
        {
            _myCollider.isTrigger = true;
        }

        private void CastArea(Transform target)
        {
            if (_areaInvoker && _areaPrefab != null)
            {
                RaycastHit hit;
                if (Physics.Raycast(target.position, Vector3.down, out hit,
                        Mathf.Infinity, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                {
                    GameObject area = Instantiate(_areaPrefab, hit.point, Quaternion.identity);
                    area.transform.position = hit.point;
                }
            }

            if (_areaInvoker && _areaPrefab == null) Debug.LogError("Prefab area is null");
        }
    }
}