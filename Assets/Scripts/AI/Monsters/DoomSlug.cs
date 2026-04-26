using Gameplay;
using Player;
using Player.AutoAttacks;
using UnityEngine;
using UnityEngine.AI;

namespace AI.Monsters
{
    public class DoomSlug : Monster
    {
        [SerializeField] private PlayerDetector _damageArea;
        [SerializeField] private MeshRenderer _myDamageRenderer;
        
        private float _timingDamage = 0.5f;
        private bool _triggerAnim;
        private bool _playerInArea;
        private bool _doingAttack;
        private float _currentFootSoundTimer;

        private void Awake()
        {
            _damage = _baseDamage;
            _hpMax = _baseHpMax;
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
            OnMonsterDie += OnDie;
        }

        protected new void OnDisable()
        {
            ClockGame.OnMonstersGrow -= Grow;
            _triggerAttack.OnPlayerDetected -= PlayerDetected;
            _damageArea.OnPlayerDetected -= PlayerInArea;
            OnMonsterDie -= OnDie;
        }
        
        private void Update()
        {
            if (!IsDead && _myTarget)
            {
                //---CDR---
                if (_currentTimeBeforAttack > 0 && _isCastReady == false)
                {
                    _currentTimeBeforAttack -= Time.deltaTime;
                    if (_currentTimeBeforAttack < 0) _currentTimeBeforAttack = 0;
                }

                if (_currentTimeBeforAttack <= 0)
                    _isCastReady = true;
                //---------

                _monsterAnimator.SetBool(IsMoving, _myNavMeshAgent.velocity != Vector3.zero);
                if (_playerDetected == false && _doingAttack == false)
                {
                    PlayerTargeting();
                    if (_myTarget && Character.Instance.transform.GetComponent<AttackNearestFoes>().enabled)
                    {
                        Chase();
                    }
                }
                else
                {
                    if (_myNavMeshAgent.enabled)
                        _myNavMeshAgent.enabled = false;
                    if ( _isCastReady && Character.Instance.transform.GetComponent<AttackNearestFoes>().enabled)
                        DoAttack();
                }
            }
            
        }
        
        private protected override void DoAttack()
        {
            _doingAttack = true;
            if (_myNavMeshAgent.enabled)
                _myNavMeshAgent.enabled = false;
            if (_timingDamage > 0)
            {
                if (_triggerAnim == false) {
                    _monsterAnimator.SetTrigger(Attack);
                    _triggerAnim = true;
                }
                if (_myDamageRenderer.enabled == false)
                {
                    _myDamageRenderer.enabled = true;
                }
                _timingDamage -= Time.deltaTime;
            }
            if (_timingDamage <= 0)
            {
                _myDamageRenderer.enabled = false;
                if (_playerInArea)
                {
                    _impactFx.SetActive(true);
                    Character.Instance.TakeDamage(_damage);
                    //Debug.Log("minotaur "+gameObject.name+" damage:"+_damage);
                    Invoke("UnactiveFX",1);
                }
                _timingDamage = 1f;
                _currentTimeBeforAttack = _attackSpeed;
                _isCastReady = false;
                _triggerAnim = false;
                _doingAttack = false;
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
        private void OnDie()
        {
            if (_myDamageRenderer.enabled)
            {
                _myDamageRenderer.enabled = false;
            }
        }
    }
}