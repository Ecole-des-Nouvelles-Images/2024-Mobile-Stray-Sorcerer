using Player;
using UnityEngine;

namespace AI.Monsters
{
    public class Minotaur : Monster
    {
        [SerializeField] private PlayerDetector _damageArea;
        
        private protected override void DoAttack()
        {
            if(_damageArea.DetectObject)
            {
                Character.Instance.TakeDamage(_damage);
            }
            _isAttacking = false;
            _currentTimeBeforAttack = _attackSpeed;
        }
    }
}