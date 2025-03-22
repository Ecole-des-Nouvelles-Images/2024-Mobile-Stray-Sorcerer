using Gameplay;
using Gameplay.GameData;
using UnityEngine;

namespace AI.Monsters
{
    public class Imp : Monster
    {
        [Header("attack setting")] [SerializeField]
        private GameObject _originAttack;

        [SerializeField] private GameObject _monsterProjectile;
        [SerializeField] private int _throwPower = 2;
        [SerializeField] private AudioSource _wingsAS;
        [SerializeField] private AudioSource _impAS;
        [SerializeField] private AudioClip[] _impSounds; // 0=>death1 1=>death2
        
        protected new void OnEnable()
        {
            ClockGame.OnMonstersGrow += Grow;
            _triggerAttack.OnPlayerDetected += PlayerDetected;
            OnMonsterDie += PlayDeathSounds;
        }

        protected new void OnDisable()
        {
            ClockGame.OnMonstersGrow -= Grow;
            _triggerAttack.OnPlayerDetected -= PlayerDetected;
            OnMonsterDie -= PlayDeathSounds;
        }
        private protected override void DoAttack()
        {
            GameObject projectile = Instantiate(_monsterProjectile, _originAttack.transform.position, Quaternion.identity);
            projectile.GetComponent<CreatureProjectile>().ShootToDestination(_myTarget.transform, _damage, _throwPower, _impactFx);
            Destroy(projectile, 5f);
            _currentTimeBeforAttack = _attackSpeed;
            _isCastReady = false;
            _monsterAnimator.SetTrigger(Attack);
        }
        private void PlayDeathSounds()
        {
            _impAS.clip = _impSounds[Random.Range(0, _impSounds.Length-1)];
            _wingsAS.Stop();
            _impAS.Play();
        }
    }
}