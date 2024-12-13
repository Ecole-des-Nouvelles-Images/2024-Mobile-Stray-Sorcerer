using System.Collections.Generic;
using System.Linq;
using Player;
using UnityEngine;

namespace UI.GameOverlay
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject _heartPrefab;
        [SerializeField] private GameObject _heartEmptyPrefab;

        [SerializeField] private Transform _layout;

        private List<Heart> _liveHP = new();

        private void OnEnable()
        {
            Character.OnPlayerSpawn += InitializeHealth;
            Character.OnHpChanged += UpdateLiveHP;
            Character.OnMaxHpChanged += IncreaseMaxHP;
            Character.OnLevelUp += RefillHealth;
        }

        private void OnDisable()
        {
            Character.OnPlayerSpawn -= InitializeHealth;
            Character.OnHpChanged -= UpdateLiveHP;
            Character.OnMaxHpChanged -= IncreaseMaxHP;
            Character.OnLevelUp -= RefillHealth;
        }

        public void InitializeHealth()
        {
            int playerMaxHp = Character.Instance.MaxHP;

            for (int i = 0; i < playerMaxHp; i++)
            {
                _liveHP.Add(Instantiate(_heartPrefab, _layout).GetComponent<Heart>());
            }
        }

        private void UpdateLiveHP(int currentHpValue)
        {
            for (int i = 0; i < _liveHP.Count; i++)
            {
                if (i < currentHpValue)
                    _liveHP[i].Fill();
                else
                    _liveHP[i].Empty();
            }
        }

        private void IncreaseMaxHP(int addedValue)
        {
            for (int i = 0; i < addedValue; i++)
            {
                _liveHP.Add(Instantiate(_heartEmptyPrefab, _layout).GetComponent<Heart>());
            }
        }

        public void RefillHealth()
        {
            for (int i = 0; i < _liveHP.Count; i++)
            {
                _liveHP[i].Fill();
            }
        }
    }
}
