using Player;
using UnityEngine;

namespace AI.Monsters
{
    public class Minotaur : Monster
    {
        [SerializeField] private PlayerDetector _damageArea;
        
        private protected override void DoAttack()
        {
            if(_myNavMeshAgent.enabled)
                _myNavMeshAgent.enabled = false;
            if(_damageArea.DetectObject)
            {
                Character.Instance.TakeDamage(_damage);
            }
        }
    }
}