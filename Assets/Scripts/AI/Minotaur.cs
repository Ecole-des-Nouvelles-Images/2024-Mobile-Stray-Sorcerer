using System;
using Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace AI
{
    public class Minotaur : MonsterBrain
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