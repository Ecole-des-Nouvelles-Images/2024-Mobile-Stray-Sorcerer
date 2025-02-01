using AI.Monsters;
using UnityEngine;
using Utils;
using Random = Unity.Mathematics.Random;

namespace Player.AutoAttacks
{
    public class AttackNearestFoes : MonoBehaviour
    {
        public static readonly int DoAttack = Animator.StringToHash("doAttack");
        public static readonly int AttackSpeed = Animator.StringToHash("attackSpeed");

        [Header("References")] [SerializeField]
        private EnemyDetector _enemyDetector;

        [SerializeField] private Transform _projectileOrigin;
        [SerializeField] private Animator _characterAnimator;
        [SerializeField] private GameObject _chargingCastFX;
        [SerializeField] private GameObject _castFX;
        [SerializeField] private GameObject _cooldownFX;
        [SerializeField] private AudioSource _castAudioSource;
        [SerializeField] private AudioClip[] _castAudioClips;// 0=>Implosion 1=>Explosion/cast

        [Header("Settings")]
        [SerializeField] private int _projectileVelocity = 5;
        [SerializeField] private float _baseCastDelay = 1.2f;

        public float Cooldown => Character.Instance.AttackCooldown;

        private GameObject _nearestFoe;
        private bool _attackIsReady = true;
        private float _currentCooldownTimer;
        private AudioSource _throwAudioSource;
        private bool _casting;
        private float _castDelay;
        private float _currentDelay;
        private float _animSpeedMult = 0.025f;
        private float _delayMult = 0.02f;
        // private float _animSpeed = 1f;

        private void Awake()
        {
            _throwAudioSource = _projectileOrigin.transform.GetComponent<AudioSource>();
            _currentCooldownTimer = Cooldown;
            _chargingCastFX.SetActive(false);
            _castFX.SetActive(false);
        }

        private void OnEnable()
        {
            Character.OnLevelUp += UpdateDelayAndAnimSpeedValue;
        }

        private void OnDisable()
        {
            Character.OnLevelUp -= UpdateDelayAndAnimSpeedValue;
        }

        private void Start()
        {
            UpdateDelayAndAnimSpeedValue();
        }

        private void Update()
        {

            if (!_attackIsReady && _currentCooldownTimer < Cooldown)
            {
                if(_chargingCastFX.activeSelf)
                    _chargingCastFX.SetActive(false);
                if(_cooldownFX.activeSelf == false)
                    _cooldownFX.SetActive(true);
                _currentCooldownTimer += Time.deltaTime;
            }

            if (_currentCooldownTimer >= Cooldown) _attackIsReady = true;
            if (_attackIsReady)
            {
                _cooldownFX.SetActive(false);
                SearchNearestFoe();
                if (_nearestFoe && _nearestFoe.transform.GetComponent<Monster>().IsDead == false && !_casting)
                {
                    if(_castFX.activeSelf)
                        _castFX.SetActive(false);
                    _chargingCastFX.SetActive(true);
                    _castAudioSource.clip = _castAudioClips[0];
                    _castAudioSource.Play();
                    _currentDelay = _castDelay;
                    _casting = true;
                    _characterAnimator.SetTrigger(DoAttack);
                }

                if (_nearestFoe && _casting && _nearestFoe.transform.GetComponent<Monster>().IsDead == false)
                    DelayBeforeCast();
            }

            if (_nearestFoe != null && _nearestFoe.activeSelf == false)
                _nearestFoe = null;
        }

        private void SearchNearestFoe()
        {
            if (_enemyDetector.EnemiesInRange.Count == 0)
                _nearestFoe = null;

            for (int i = 0; i < _enemyDetector.EnemiesInRange.Count; i++)
            {
                if (!_nearestFoe && _enemyDetector.EnemiesInRange[i] && Helper.DirectViewBetweenTwoObject(gameObject, _enemyDetector.EnemiesInRange[i], false))
                {
                    _nearestFoe = _enemyDetector.EnemiesInRange[i];
                    return;
                }

                if (_nearestFoe && _enemyDetector.EnemiesInRange[i])
                {
                    if (Vector3.Distance(transform.position, _nearestFoe.transform.position) > Vector3.Distance(transform.position, _enemyDetector.EnemiesInRange[i].transform.position)
                        && Helper.DirectViewBetweenTwoObject(gameObject, _enemyDetector.EnemiesInRange[i], false))
                    {
                        _nearestFoe = _enemyDetector.EnemiesInRange[i];
                    }
                }
            }
        }
        private void DelayBeforeCast()
        {
            if (_currentDelay > 0)
            {
                _currentDelay -= Time.deltaTime;
                return;
            }

            CastSpell();
        }
        private void CastSpell()
        {
            _castFX.SetActive(true);
            _castAudioSource.clip = _castAudioClips[1];
            _castAudioSource.Play();
            GameObject projectile = Instantiate(Character.Instance.CurrentSpellSo.ProjectilePrefab, _projectileOrigin.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody>().AddForce((_nearestFoe.transform.position - projectile.transform.position) * _projectileVelocity, ForceMode.Impulse);
            _attackIsReady = false;
            _currentCooldownTimer = 0;

            Destroy(projectile, 5f);
            _casting = false;
        }

        private void UpdateDelayAndAnimSpeedValue()
        {
            int characterLevel = Character.Instance.Level;
            _castDelay = _baseCastDelay - _delayMult * characterLevel;
            _characterAnimator.SetFloat(AttackSpeed, 1 + _animSpeedMult * characterLevel);
        }
    }
}
