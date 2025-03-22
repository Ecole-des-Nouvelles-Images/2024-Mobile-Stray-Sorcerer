using Gameplay;
using Gameplay.GameData;
using Player;
using Player.AutoAttacks;
using UnityEngine;
using UnityEngine.AI;

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
        
        private float _timingDamage = 0.5f;
        private bool _triggerAnim;
        private bool _playerInArea;
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
            OnMonsterTakeDamage += HurtSound;
            OnMonsterDie += OnDieSound;
        }

        protected new void OnDisable()
        {
            ClockGame.OnMonstersGrow -= Grow;
            _triggerAttack.OnPlayerDetected -= PlayerDetected;
            _damageArea.OnPlayerDetected -= PlayerInArea;
            OnMonsterTakeDamage -= HurtSound;
            OnMonsterDie -= OnDieSound;
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
            
            if (_myNavMeshAgent.enabled)
                _myNavMeshAgent.enabled = false;
            if (_timingDamage > 0)
            {
                if (_triggerAnim == false) {
                    _monsterAnimator.SetTrigger(Attack);
                    _minotaurAS.clip = _minotaurClips[2];
                    _minotaurAS.Play();
                    _triggerAnim = true;
                }
                _timingDamage -= Time.deltaTime;
            }
            if (_timingDamage <= 0)
            {
                if (_playerInArea)
                {
                    _impactFx.SetActive(true);
                    _impactAS.Play();
                    Character.Instance.TakeDamage(_damage);
                    //Debug.Log("minotaur "+gameObject.name+" damage:"+_damage);
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

        private void HurtSound()
        {
            _minotaurAS.clip = _minotaurClips[0];
            _minotaurAS.Play();
        }

        private void OnDieSound()
        {
            _minotaurAS.clip = _minotaurClips[1];
            _minotaurAS.Play();
        }
    }
}