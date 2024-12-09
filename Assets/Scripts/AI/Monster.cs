using Player;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace AI
{
   public abstract class Monster : MonoBehaviour
   {
      
      [Header("Stats")]
      [SerializeField] protected int _damage;
      
      [SerializeField] private int _hpMax;
      [SerializeField] private float _hpGrowingFactor;
      [SerializeField] private float _damageGrowingFactor;
      [SerializeField] private float _speed;
      [SerializeField] private float _acceleration;
      [SerializeField] private float _attackSpeed = 1;
      [SerializeField] private int _rangeValue;
      [Header("Drop")]
      [SerializeField] private GameObject[] _dropPrefabs;
      [SerializeField] private GameObject _xpPrefab;
      [Header("UI")]
      [SerializeField] private Canvas _myCanvas;
      [SerializeField] private Slider _currentHpSlider;
      [Header("References")]
      [SerializeField] private PlayerDetector _triggerAttack;
      
      public int CurrentHp { get ; private set; }

      protected GameObject _myTarget;
      protected GameObject _myRaycastTarget;
      
      private NavMeshAgent _myNavMeshAgent;
      private float _currentTimeBeforAttack;
      private bool _isAttacking;

      private void Awake()
      {
         _myNavMeshAgent = transform.GetComponent<NavMeshAgent>();
         _myTarget = null;
         _myRaycastTarget = Character.Instance.EnnemyRaycastTarget.gameObject;
      }

      private void Start()
      {
         CurrentHp = _hpMax;
         _currentHpSlider.value = _hpMax;
         _myNavMeshAgent.speed = _speed;
         _myNavMeshAgent.acceleration = _acceleration;
         _currentHpSlider.maxValue = _hpMax;
      }

      private void Update()
      {
         if (_myTarget != null && _triggerAttack.DetectObject == false)
         {
            _myNavMeshAgent.SetDestination(_myTarget.transform.position);
         }
         if (_isAttacking && _currentTimeBeforAttack <= 0 && _triggerAttack.DetectObject )
         {
            //play anim
            DoAttack();
            _isAttacking = false;
            _currentTimeBeforAttack = _attackSpeed;
         }
         if (_isAttacking && _currentTimeBeforAttack > 0)
         {
            _currentTimeBeforAttack -= Time.deltaTime;
         }
         if ( _triggerAttack.DetectObject  && _isAttacking == false )
         {
            _isAttacking = true;
         }

         if (_triggerAttack.DetectObject )
         {
            _myNavMeshAgent.SetDestination(transform.position);
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
         }
      }

      private protected abstract void DoAttack();

      private void Death()
      {
         if (_dropPrefabs.Length > 0)
         {
            Instantiate(_dropPrefabs[Random.Range(0, _dropPrefabs.Length)], transform.position, Quaternion.identity);
         }
         Instantiate(_xpPrefab, transform.position, Quaternion.identity);
         gameObject.SetActive(false);
         //Destroy(gameObject);
      }

      public void DefineTarget(GameObject target)
      {
         _myTarget = target;
      }
   }
}
