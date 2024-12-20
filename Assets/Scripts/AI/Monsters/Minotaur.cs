using Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Utils;

namespace AI.Monsters
{
    public class Minotaur : Monster
    {
        [SerializeField] private PlayerDetector _damageArea;
        
        private float _timingDamage = 0.5f;
        private bool _triggerAnim;
        private bool _playerInArea;

        private void Awake()
        {
            _myNavMeshAgent = transform.GetComponent<NavMeshAgent>();
            _myTarget = null;
            _rb = GetComponent<Rigidbody>();
            _isCastReady = true;
            _impactFx.SetActive(false);
        }
        protected new void OnEnable()
        {
            ClockGame.OnMonstersGrow += Grow;
            _triggerAttack.OnPlayerDetected += PlayerDetected;
            _damageArea.OnPlayerDetected += PlayerInArea;
        }

        protected new void OnDisable()
        {
            ClockGame.OnMonstersGrow -= Grow;
            _triggerAttack.OnPlayerDetected -= PlayerDetected;
            _damageArea.OnPlayerDetected -= PlayerInArea;
        }
        private protected override void DoAttack()
        {
            
            if (_myNavMeshAgent.enabled)
                _myNavMeshAgent.enabled = false;
            if (_timingDamage > 0)
            {
                if (_triggerAnim == false) {
                    _monsterAnimator.SetTrigger(Attack);
                    _triggerAnim = true;
                }
                _timingDamage -= Time.deltaTime;
            }
            if (_timingDamage <= 0)
            {
                if (_playerInArea)
                {
                    _impactFx.SetActive(true);
                    Character.Instance.TakeDamage(_damage);
                    Invoke("UnactiveFX",1);
                }
                _timingDamage = 0.5f;
                _currentTimeBeforAttack = _attackSpeed;
                _isCastReady = false;
                _triggerAnim = false;
            }
        }

        private void PlayerInArea(bool playerDetected)
        {
            _playerInArea = playerDetected;
        }

        private void UnactiveFX()
        {
            _impactFx.SetActive(false);
        }
    }
}