using System.Collections;
using System.Collections.Generic;
using Gameplay.GameData;
using Player;
using Player.AutoAttacks;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace AI.Monsters
{
    public abstract class Monster : MonoBehaviour
    {
        public static readonly int IsMoving = Animator.StringToHash("isMoving");
        public static readonly int Attack = Animator.StringToHash("attack");
        public static readonly int Hurt = Animator.StringToHash("hurt");
        public static readonly int DoDeath = Animator.StringToHash("death");
        public static readonly int Dissolve = Shader.PropertyToID("_State");

        [Header("Stats")] [SerializeField] protected int _damage;

        [SerializeField] private int _hpMax;
        [SerializeField] private float _hpGrowingFactor;
        [SerializeField] private float _damageGrowingFactor;
        [SerializeField] private float _speed;
        [SerializeField] private float _acceleration;
        [SerializeField] protected float _attackSpeed = 1;
        
        [Header("Drop")] 
        [SerializeField] private GameObject[] _dropPrefabs;
        [SerializeField] private GameObject _xpPrefab;

        [Header("References")] 
        [SerializeField] protected PlayerDetector _triggerAttack;
        [SerializeField] protected Animator _monsterAnimator;
        [SerializeField] protected GameObject _impactFx;
        [SerializeField] private List<Renderer> _renderers;
        
        [Header("Timer")] 
        [SerializeField] private float _deathAnimationDuration = 5;

        public int CurrentHp { get; private set; }
        public bool IsDead { get; private set; }

        protected GameObject _myTarget;
        protected Rigidbody _rb;
        protected NavMeshAgent _myNavMeshAgent;
        protected float _currentTimeBeforAttack;
        protected bool _isCastReady;
        private bool _playerInRange;
        private bool _playerDetected;

        private void Awake()
        {
            _myNavMeshAgent = transform.GetComponent<NavMeshAgent>();
            _myTarget = null;
            _rb = GetComponent<Rigidbody>();
            _isCastReady = true;
        }

        protected void OnEnable()
        {
            ClockGame.OnMonstersGrow += Grow;
            _triggerAttack.OnPlayerDetected += PlayerDetected;
        }

        protected void OnDisable()
        {
            ClockGame.OnMonstersGrow -= Grow;
            _triggerAttack.OnPlayerDetected -= PlayerDetected;
        }

        private void Start()
        {
            CurrentHp = _hpMax;
            _myNavMeshAgent.speed = _speed;
            _myNavMeshAgent.acceleration = _acceleration;
            Standby();
            Grow(ClockGame.Instance.GrowingLevel);
            
        }

        private void Update()
        {

            if (!IsDead && _myTarget)
            {
                //---timer---
                if (_currentTimeBeforAttack > 0 && _isCastReady == false)
                {
                    _currentTimeBeforAttack -= Time.deltaTime;
                    if (_currentTimeBeforAttack < 0) _currentTimeBeforAttack = 0;
                }

                if (_currentTimeBeforAttack <= 0)
                    _isCastReady = true;
                //---------

                _monsterAnimator.SetBool(IsMoving, _myNavMeshAgent.velocity != Vector3.zero);

                if (_myTarget && _playerDetected == false && Character.Instance.transform.GetComponent<AttackNearestFoes>().enabled)
                    Chase();
                if (_playerDetected && _isCastReady && Character.Instance.transform.GetComponent<AttackNearestFoes>().enabled) DoAttack();
                if (_playerDetected)
                    PlayerTargeting();
            }
        }

        public void TakeDamage(int damage)
        {
            _myNavMeshAgent.velocity = Vector3.zero;
            CurrentHp -= damage;
            if (CurrentHp <= 0)
            {
                StartCoroutine(DeathAnimationCoroutine());
                return;
            }

            _monsterAnimator.SetTrigger(Hurt);
        }

        private protected abstract void DoAttack();

        protected void PlayerDetected(bool playerDetected)
        {
            _playerDetected = playerDetected;
        }

        protected void Grow(int growMult)
        {
            float hpGrowth = _hpMax * (_hpGrowingFactor * growMult);
            _hpMax += (int)hpGrowth;
            float damageGrowth = _damage * (_damageGrowingFactor * growMult);
            _damage += (int)damageGrowth;
        }

        private void PlayerTargeting()
        {
            if (Character.Instance)
            {
                if (_myNavMeshAgent.enabled)
                    _myNavMeshAgent.enabled = false;
                Quaternion rotation = Quaternion.LookRotation(_myTarget.transform.position - transform.position, Vector3.up);
                _rb.MoveRotation(rotation);
            }
        }

        public void DefineTarget(GameObject target)
        {
            _myTarget = target;
        }

        private void Standby()
        {
            _myTarget = null;
            _myNavMeshAgent.enabled = false;
        }

        private void Chase()
        {
            if (_myNavMeshAgent.enabled == false)
                _myNavMeshAgent.enabled = true;
            _myNavMeshAgent.SetDestination(_myTarget.transform.position);
        }

        private IEnumerator DeathAnimationCoroutine()
        {
            float t = 0f;
            List<Material> materials = new();
            IsDead = true;
            Standby();
            _monsterAnimator.SetTrigger(DoDeath);
            gameObject.GetComponent<Collider>().enabled = false;
            if(DataCollector.Instance)
                DataCollector.OnMonsterDeath?.Invoke();
            if (_dropPrefabs.Length > 0 && Random.Range(1,6)==6)
            {
                Vector3 position = new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z);
                Instantiate(_dropPrefabs[Random.Range(0, _dropPrefabs.Length-1)], position, Quaternion.identity);
            }
            Instantiate(_xpPrefab, transform.position, Quaternion.identity);
            

            while (t < 1)
            {
                t += Time.deltaTime / _deathAnimationDuration;

                foreach (Renderer rd in _renderers)
                {
                    rd.GetMaterials(materials); // Copy originals

                    foreach (Material material in materials) {
                        material.SetFloat(Dissolve, Mathf.Lerp(0, 1, t));
                    }

                    materials.Clear();
                }

                yield return null;
            }

            yield return new WaitForSeconds(.5f);
            Destroy(gameObject);
        }
    }
}