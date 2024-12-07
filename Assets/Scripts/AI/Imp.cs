using UnityEngine;

namespace AI
{
    public class Imp : Monster
    {
        [Header("attack setting")]
        [SerializeField] private GameObject _originAttack;
        [SerializeField] private GameObject _monsterProjectile;
        [SerializeField] private int _throwPower = 2;
        private protected override void DoAttack()
        {
            GameObject projectile = Instantiate(_monsterProjectile, _originAttack.transform.position, Quaternion.identity);
            projectile.GetComponent<CreatureProjectile>().Damage = _damage;
            projectile.GetComponent<Rigidbody>().AddForce((_myRaycastTarget.transform.position - projectile.transform.position) * _throwPower, ForceMode.Impulse);
            Destroy(projectile, 5f);
        }
    }
}