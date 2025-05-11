using Gameplay;
using Gameplay.GameData;
using Player;
using Player.AutoAttacks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;
using Utils;

namespace AI.Monsters
{
    public class Minotaur : Monster
    {
        [SerializeField] private PlayerDetector _damageArea;
        [SerializeField] private AudioSource _minotaurAS;
        [SerializeField] private AudioSource _impactAS;
        [SerializeField] private AudioSource _footAS;
        [SerializeField] private AudioClip[] _minotaurClips;// 0=>Hurt 1=>death 2=>Woosh 3=>attack 
        [SerializeField] private AudioClip[] _footClips;// 0=>Step1 1=>Step2 2=>Step3 3=>Step4
        [SerializeField] private float _footSoundTimer;
        [SerializeField] private MeshRenderer _myDamageRenderer;
        
        private float _timingDamage;
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
            _myDamageRenderer.enabled = false;
        }
        protected new void OnEnable()
        {
            ClockGame.OnMonstersGrow += Grow;
            _triggerAttack.OnPlayerDetected += PlayerDetected;
            _damageArea.OnPlayerDetected += PlayerInArea;
            OnMonsterTakeDamage += HurtSound;
            OnMonsterDie += OnDie;
        }

        protected new void OnDisable()
        {
            ClockGame.OnMonstersGrow -= Grow;
            _triggerAttack.OnPlayerDetected -= PlayerDetected;
            _damageArea.OnPlayerDetected -= PlayerInArea;
            OnMonsterTakeDamage -= HurtSound;
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
                
                
               
                
                if (_myNavMeshAgent.velocity != Vector3.zero)
                {
                    if(_currentFootSoundTimer >= _footSoundTimer)
                    {
                        _footAS.clip = _footClips[Random.Range(0, _footClips.Length - 1)];
                        _footAS.Play();
                        _currentFootSoundTimer = 0;
                    }
                    else
                    {
                        _currentFootSoundTimer += Time.deltaTime;
                    }
                }
                else if(_currentFootSoundTimer > 0)
                    _currentFootSoundTimer = 0;
            }
            
        }
        private protected override void DoAttack()
        {
            _doingAttack = true;
            if (_myNavMeshAgent.enabled)
                _myNavMeshAgent.enabled = false;
            if (_timingDamage < 0.6f)
            {
                if (_triggerAnim == false) {
                    _monsterAnimator.SetTrigger(Attack);
                    _minotaurAS.clip = _minotaurClips[2];
                    _minotaurAS.Play();
                    _triggerAnim = true;
                }
                if (_myDamageRenderer.enabled == false)
                {
                    _myDamageRenderer.enabled = true;
                }
                _timingDamage += Time.deltaTime;
            }
            if (_timingDamage >= 0.6f)
            {
                _myDamageRenderer.enabled = false;
                if (_playerInArea)
                {
                    _impactFx.SetActive(true);
                    _impactAS.Play();
                    Character.Instance.TakeDamage(_damage);
                    //Debug.Log("minotaur "+gameObject.name+" damage:"+_damage);
                    Invoke("UnactiveFX",1);
                }
                _timingDamage = 0;
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

        private void HurtSound()
        {
            _minotaurAS.clip = _minotaurClips[0];
            _minotaurAS.Play();
        }

        private void OnDie()
        {
            _minotaurAS.clip = _minotaurClips[1];
            _minotaurAS.Play();
            if (_myDamageRenderer.enabled)
            {
                _myDamageRenderer.enabled = false;
            }
        }
    }
}