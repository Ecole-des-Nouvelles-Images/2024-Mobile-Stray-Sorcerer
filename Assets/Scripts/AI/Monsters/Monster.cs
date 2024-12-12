using Player;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

using Utils;

namespace AI
{
   public abstract class Monster : MonoBehaviour
   {
      public static readonly int IsMoving = Animator.StringToHash("isMoving");
      public static readonly int Attack = Animator.StringToHash("attack");
      public static readonly int Hurt = Animator.StringToHash("hurt");
      public static readonly int DoDeath = Animator.StringToHash("death");
      
      public bool IsDead;
      
      [Header("Stats")]
      [SerializeField] protected int _damage;
      
      [SerializeField] private int _hpMax;
      [SerializeField] private float _hpGrowingFactor;
      [SerializeField] private float _damageGrowingFactor;
      [SerializeField] private float _speed;
      [SerializeField] private float _acceleration;
      [SerializeField] protected float _attackSpeed = 1;
      [Header("Drop")]
      [SerializeField] private GameObject[] _dropPrefabs;
      [SerializeField] private GameObject _xpPrefab;
      [Header("UI")]
      [SerializeField] private Canvas _myCanvas;
      [SerializeField] private Slider _currentHpSlider;
      [Header("References")]
      [SerializeField] private PlayerDetector _triggerAttack;
      [SerializeField] private Animator _monsterAnimator;

      public int CurrentHp { get ; private set; }
      
      protected GameObject _myTarget;
      protected GameObject _myRaycastTarget;
      protected Rigidbody _rb;
      protected NavMeshAgent _myNavMeshAgent;
      protected float _currentTimeBeforAttack;
      protected bool _isAttacking;
      private void OnEnable()
      {
         ClockGame.OnMonstersGrow += Grow;
      }

      private void OnDisable()
      {
         ClockGame.OnMonstersGrow -= Grow;
      }
      
      private void Awake()
      {
         _myNavMeshAgent = transform.GetComponent<NavMeshAgent>();
         _myTarget = null;
         _rb = GetComponent<Rigidbody>();
      }

      private void Start()
      {
         CurrentHp = _hpMax;
         _currentHpSlider.value = _hpMax;
         _myNavMeshAgent.speed = _speed;
         _myNavMeshAgent.acceleration = _acceleration;
         _currentHpSlider.maxValue = _hpMax;
         Standby();
      }

      private void Update()
      {
         if(!IsDead)
         {
            _monsterAnimator.SetBool(IsMoving, _myNavMeshAgent.velocity != Vector3.zero);
            if (_isAttacking)
            {
               PlayerTargeting();
            }
            
            if (_myTarget != null && _triggerAttack.DetectObject == false)
                Chase();

            if (_isAttacking && _currentTimeBeforAttack <= 0 && _triggerAttack.DetectObject && Character.Instance.transform.GetComponent<PlayerInput>().enabled)
            {
               _monsterAnimator.SetTrigger(Attack);
               DoAttack();
            }

            if (_isAttacking && _currentTimeBeforAttack > 0)
            {
               _currentTimeBeforAttack -= Time.deltaTime;
            }

            if (_triggerAttack.DetectObject && _isAttacking == false)
            {
               _isAttacking = true;
            }
         }
      }

      public void TakeDamage(int damage)
      {
         Debug.Log("MONSTER: damage taken" + damage);
         _myNavMeshAgent.velocity = Vector3.zero;
         CurrentHp -= damage;
         _currentHpSlider.value = CurrentHp;
         if (CurrentHp <= 0)
         {
            Death();
            return;
         }
         _monsterAnimator.SetTrigger(Hurt);
      }

      private protected abstract void DoAttack();

      private void Death()
      {
         IsDead = true;
         Standby();
         _monsterAnimator.SetTrigger(DoDeath);
         gameObject.GetComponent<Collider>().enabled = false;
         if (_dropPrefabs.Length > 0)
         {
            Instantiate(_dropPrefabs[Random.Range(0, _dropPrefabs.Length)], transform.position, Quaternion.identity);
         }
         Instantiate(_xpPrefab, transform.position, Quaternion.identity);
         Invoke("UnactiveFoe",3);
      }

      private void Grow(int growMult)
      {
         _hpMax =  (int)(_hpMax * (1 + _hpGrowingFactor*growMult));
         _damage = (int)(_damage * (1 + _damageGrowingFactor*growMult));
      }
      private void UnactiveFoe()
      {
         gameObject.SetActive(false);
      }

      private void PlayerTargeting()
      {
         if(_myNavMeshAgent.enabled)
            _myNavMeshAgent.enabled = false;
         Quaternion rotation = Quaternion.LookRotation(_myTarget.transform.position - transform.position,Vector3.up);
         _rb.MoveRotation(rotation);
      }
      public void DefineTarget(GameObject target)
      {
         _myTarget = target;
         _myRaycastTarget = Character.Instance.EnnemyRaycastTarget.gameObject;
      }

      private void Standby()
      {
         _myTarget = null;
         _myNavMeshAgent.enabled = false;
      }

      private void Chase()
      {
         if(_myNavMeshAgent.enabled == false)
            _myNavMeshAgent.enabled = true;
         _myNavMeshAgent.SetDestination(_myTarget.transform.position);
      }
      
   }
}
