using Player;
using UnityEngine;
using UnityEngine.AI;

namespace AI.Monsters
{
    public class Minotaur : Monster
    {
        [SerializeField] private PlayerDetector _damageArea;
        
        private float _timingDamage = 0.2f;
        private bool _triggerAnim;

        private void Awake()
        {
            _myNavMeshAgent = transform.GetComponent<NavMeshAgent>();
            _myTarget = null;
            _rb = GetComponent<Rigidbody>();
            _isCastReady = true;
            _impactFx.SetActive(false);
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
                if (_damageArea.DetectObject)
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

        private void UnactiveFX()
        {
            _impactFx.SetActive(false);
        }
    }
}