using Player;
using UnityEngine;

namespace AI.Monsters
{
    public class Minotaur : Monster
    {
        [SerializeField] private PlayerDetector _damageArea;
        private float _timingDamage = 0.5f;

        private protected override void DoAttack()
        {
            if (_myNavMeshAgent.enabled)
                _myNavMeshAgent.enabled = false;
            if (_timingDamage > 0) _timingDamage -= Time.deltaTime;
            if (_timingDamage <= 0)
            {
                if (_damageArea.DetectObject) Character.Instance.TakeDamage(_damage);
                _timingDamage = 0.5f;
                _currentTimeBeforAttack = _attackSpeed;
                _isCastReady = false;
                _monsterAnimator.SetTrigger(Attack);
            }
        }
    }
}