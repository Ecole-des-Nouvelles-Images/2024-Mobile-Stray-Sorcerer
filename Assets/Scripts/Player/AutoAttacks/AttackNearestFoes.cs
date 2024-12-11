using UnityEngine;
using Utils;

namespace Player.AutoAttacks
{
    public class AttackNearestFoes : MonoBehaviour
    {
        public static readonly int Doattack = Animator.StringToHash("doAttack");
        
        [Header("References")]
        [SerializeField] private EnemyDetector _enemyDetector;
        [SerializeField] private Transform _projectileOrigin;
        [SerializeField] private Animator _characterAnimator;
        
        [Header("Settings")]
        [SerializeField] private int _projectileVelocity = 5;
        [SerializeField] private float _castDelay = 1;

        private GameObject _nearestFoe;
        private bool _attackIsReady = true;
        private float _cooldown = 1;
        private float _currentCooldownTimer;
        private AudioSource _throwAudioSource;
        private bool _casting;
        private float _currentDelay;

        public float Cooldown => Character.Instance.AttackCooldown;

        private void Awake()
        {
            _throwAudioSource = _projectileOrigin.transform.GetComponent<AudioSource>();
            _currentCooldownTimer = Cooldown;
        }

        void Update()
        {
            if (!_attackIsReady && _currentCooldownTimer < Cooldown) _currentCooldownTimer += Time.deltaTime;
            if (_currentCooldownTimer >= Cooldown) _attackIsReady = true;
            if (_attackIsReady)
            {
                SearchNearestFoe();
                if (_nearestFoe && !_casting)
                {
                    _currentDelay = _castDelay;
                    _casting = true;
                    _characterAnimator.SetTrigger(Doattack);
                }
                if (_nearestFoe && _casting )
                {
                    DelayBeforCast();
                }
            }
            if (_nearestFoe!= null && _nearestFoe.activeSelf == false)
                _nearestFoe = null;
        }

        private void SearchNearestFoe()
        {
            if (_enemyDetector.EnemiesInRange.Count == 0)
                _nearestFoe = null;

            for (int i = 0; i < _enemyDetector.EnemiesInRange.Count; i++)
            {
                if (!_nearestFoe && Helper.DirectViewBetweenTwoObject(gameObject, _enemyDetector.EnemiesInRange[i], false))
                {
                    _nearestFoe = _enemyDetector.EnemiesInRange[i];
                    return;
                }

                if (_nearestFoe
                    && Vector3.Distance(transform.position, _nearestFoe.transform.position) > Vector3.Distance(transform.position, _enemyDetector.EnemiesInRange[i].transform.position)
                    && Helper.DirectViewBetweenTwoObject(gameObject, _enemyDetector.EnemiesInRange[i], false))
                {
                    _nearestFoe = _enemyDetector.EnemiesInRange[i];
                }
            }
        }

        private void DelayBeforCast()
        {
            
            if(_currentDelay > 0)
            {
                _currentDelay -= Time.deltaTime;
                //Debug.Log(delayTime);
                return;
            }
            CastSpell();
        }

        private void CastSpell()
        {
            GameObject projectile = Instantiate(Character.Instance.CurrentSpell.ProjectilePrefab, _projectileOrigin.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody>().AddForce((_nearestFoe.transform.position - projectile.transform.position) * _projectileVelocity, ForceMode.Impulse);
            _attackIsReady = false;
            _currentCooldownTimer = 0;
            Destroy(projectile, 5f);
            _casting = false;
        }
    }
}