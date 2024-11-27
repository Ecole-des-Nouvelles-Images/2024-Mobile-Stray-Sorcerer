using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Player.AutoAttacks {
    public class AttackNearestFoes : MonoBehaviour{
        [SerializeField] private int firePower = 5;
        [SerializeField] private EnemyDetector enemyDetector;
        [SerializeField] private Image cooldownDisplay;
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private Transform projectileOrigin;

        private GameObject _nearestFoe;
        private bool _attackIsReady = true;
        private float _cooldown = 1;
        private float _currentCooldownTimer;
     
        void Update() {
            // cooldownDisplay.fillAmount = Helper.LoadFactorCalculation(_currentCooldownTimer,_cooldown);
            if (!_attackIsReady && _currentCooldownTimer >= _cooldown)_currentCooldownTimer = 0;
            if (!_attackIsReady && _currentCooldownTimer < _cooldown) _currentCooldownTimer += Time.deltaTime;
            if (_currentCooldownTimer >= _cooldown) _attackIsReady = true;
            if (_attackIsReady) {
                SearchNearestFoe();
                if (_nearestFoe != null) DoAttack();
            }
        }
    
        private void SearchNearestFoe() {
            if(enemyDetector.EnemiesInRange.Count == 0)_nearestFoe = null;
            for (int i = 0; i < enemyDetector.EnemiesInRange.Count; i++) {
                if (_nearestFoe == null &&
                    Helper.DirectViewBetweenTwoObject(gameObject, enemyDetector.EnemiesInRange[i], false))
                {
                    _nearestFoe = enemyDetector.EnemiesInRange[i];
                    return;
                }
                    
                if (_nearestFoe != null &&
                    Helper.DistanceCalculator(transform.position, _nearestFoe.transform.position) >
                    Helper.DistanceCalculator(transform.position, enemyDetector.EnemiesInRange[i].transform.position) 
                    &&
                    Helper.DirectViewBetweenTwoObject(gameObject, enemyDetector.EnemiesInRange[i], false))
                {
                    _nearestFoe = enemyDetector.EnemiesInRange[i];
                }
            }
        }

        private void DoAttack()
        {
            //animation
            //son?
            var o = Instantiate(projectilePrefab, projectileOrigin.position, Quaternion.identity);
            o.transform.GetComponent<Rigidbody>().AddForce((_nearestFoe.transform.position-o.transform.position)
                                                           * firePower, ForceMode.Impulse);
            _attackIsReady = false;
        }
        
    }
}
