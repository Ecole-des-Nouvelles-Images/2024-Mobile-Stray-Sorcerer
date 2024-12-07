using System;
using UnityEngine;
using Utils;

namespace Player.AutoAttacks
{
    public class AttackNearestFoes : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private EnemyDetector _enemyDetector;
        [SerializeField] private Transform _projectileOrigin;
        
        [Header("Settings")]
        [SerializeField] private int _projectileVelocity = 5;

        private GameObject _nearestFoe;
        private bool _attackIsReady = true;
        private float _cooldown = 1;
        private float _currentCooldownTimer;
        private AudioSource _throwAudioSource;

        private void Awake()
        {
            _throwAudioSource = _projectileOrigin.transform.GetComponent<AudioSource>();
        }

        void Update()
        {
            if (!_attackIsReady && _currentCooldownTimer >= _cooldown) _currentCooldownTimer = 0;
            if (!_attackIsReady && _currentCooldownTimer < _cooldown) _currentCooldownTimer += Time.deltaTime;
            if (_currentCooldownTimer >= _cooldown) _attackIsReady = true;
            if (_attackIsReady)
            {
                SearchNearestFoe();
                if (_nearestFoe) 
                    DoAttack();
            }
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

        private void DoAttack()
        {
            GameObject projectile = Instantiate(Character.Instance.CurrentSpell.ProjectilePrefab, _projectileOrigin.position, Quaternion.identity);

            projectile.GetComponent<Rigidbody>().AddForce((_nearestFoe.transform.position - projectile.transform.position) * _projectileVelocity, ForceMode.Impulse);
            _attackIsReady = false;
            Destroy(projectile, 5f);
        }
    }
}