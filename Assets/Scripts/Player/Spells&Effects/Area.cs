using AI.Monsters;
using Player.AutoAttacks;
using UnityEngine;

namespace Player.Spells_Effects
{
    public class Area : MonoBehaviour
    {
        [SerializeField] private EnemyDetector _enemyDetector;
        [SerializeField] private int _areaDamage;
        [SerializeField] private float _timeToDealDamage;

        private float _timer;

        private void Update()
        {
            if (_timer < _timeToDealDamage)
                _timer += Time.deltaTime;
            if (_timer >= _timeToDealDamage)
            {
                for (int i =0; i > _enemyDetector.EnemiesInRange.Count;i++)
                {
                    if(_enemyDetector.EnemiesInRange[i])
                        _enemyDetector.EnemiesInRange[i].GetComponent<Monster>().TakeDamage(_areaDamage + (int)Character.Instance.SpellPower);
                }
            }
        }
    }
}
