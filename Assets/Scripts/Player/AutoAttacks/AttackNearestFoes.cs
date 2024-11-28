using UnityEngine;
using Utils;

namespace Player.AutoAttacks
{
    public class AttackNearestFoes : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private EnemyDetector _enemyDetector;
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private Transform _projectileOrigin;

        [Header("Settings")]
        [SerializeField] private int _firePower = 5;

        private GameObject _nearestFoe;
        private bool _attackIsReady = true;
        private float _cooldown = 1;
        private float _currentCooldownTimer;

        void Update()
        {
            if (!_attackIsReady && _currentCooldownTimer >= _cooldown) _currentCooldownTimer = 0;
            if (!_attackIsReady && _currentCooldownTimer < _cooldown) _currentCooldownTimer += Time.deltaTime;
            if (_currentCooldownTimer >= _cooldown) _attackIsReady = true;
            if (_attackIsReady)
            {
                SearchNearestFoe();
                if (_nearestFoe) DoAttack();
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
            GameObject o = Instantiate(_projectilePrefab, _projectileOrigin.position, Quaternion.identity);
            o.transform.GetComponent<Rigidbody>().AddForce((_nearestFoe.transform.position - o.transform.position)
                                                           * _firePower, ForceMode.Impulse);
            _attackIsReady = false;
        }
    }
}