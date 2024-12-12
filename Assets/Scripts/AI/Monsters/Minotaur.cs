using Player;
using UnityEngine;

namespace AI
{
    public class Minotaur : Monster
    {
        [SerializeField] private PlayerDetector _damageArea;
        private bool _isDealingDamage;
        
        private protected override void DoAttack()
        {
            if(_damageArea.DetectObject)
                Character.Instance.TakeDamage(_damage);
        }
    }
}