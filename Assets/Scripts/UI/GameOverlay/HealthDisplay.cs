using System.Collections.Generic;
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

        private Coroutine _hpUpdateCoroutine;

        private void OnEnable()
        {
            Character.OnHpChanged += UpdateLiveHP;
            Character.OnMaxHpChanged += UpdateMaxHP;
        }

        private void OnDisable()
        {
            Character.OnHpChanged -= UpdateLiveHP;
            Character.OnMaxHpChanged -= UpdateMaxHP;
        }

        private void UpdateLiveHP(int currentHpValue)
        {
            for (int i = 0; i < _liveHP.Count; i++)
                if (i < currentHpValue)
                    _liveHP[i].Fill();
                else
                    _liveHP[i].Empty();
        }

        private void UpdateMaxHP(int currentValue)
        {
            Debug.Log("Upgrade Max HP");

            foreach (Transform heart in _layout.transform)
                Destroy(heart.gameObject);

            _liveHP.Clear();

            for (int i = 0; i < currentValue; i++)
            {
                GameObject heart = Instantiate(_heartPrefab, _layout);
                _liveHP.Add(heart.GetComponentInChildren<Heart>());
            }
        }

        public void RefillHealth()
        {
            for (int i = 0; i < _liveHP.Count; i++) _liveHP[i].Fill();
        }
    }
}