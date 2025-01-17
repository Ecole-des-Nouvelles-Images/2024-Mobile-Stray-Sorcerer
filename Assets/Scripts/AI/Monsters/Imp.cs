using UnityEngine;

namespace AI.Monsters
{
    public class Imp : Monster
    {
        [Header("attack setting")] [SerializeField]
        private GameObject _originAttack;

        [SerializeField] private GameObject _monsterProjectile;
        [SerializeField] private int _throwPower = 2;

        private protected override void DoAttack()
        {
            GameObject projectile = Instantiate(_monsterProjectile, _originAttack.transform.position, Quaternion.identity);
            projectile.GetComponent<CreatureProjectile>().ShootToDestination(_myTarget.transform, _damage, _throwPower, _impactFx);
            Destroy(projectile, 5f);
            _currentTimeBeforAttack = _attackSpeed;
            _isCastReady = false;
            _monsterAnimator.SetTrigger(Attack);
        }
    }
}