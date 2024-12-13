using System;
using Player;
using UnityEngine;

namespace UI.GameOverlay
{
    public class HeartBarre : MonoBehaviour
    {
        [SerializeField] private GameObject[] _backgroundsHeart;
        [SerializeField] private GameObject[] _halfHearts;

        private void OnEnable()
        {
            Character.OnHpChanged += UpdateCurrentHpDisplay;
            Character.OnMaxHpChanged += UpdateCurrentMaxHpDisplay;
        }

        private void OnDisable()
        {
            Character.OnHpChanged -= UpdateCurrentHpDisplay;
            Character.OnMaxHpChanged -= UpdateCurrentMaxHpDisplay;
        }

        private void Start()
        {
            UpdateCurrentHpDisplay(Character.Instance.HP);
            UpdateCurrentMaxHpDisplay(Character.Instance.MaxHP);
        }

        private void UpdateCurrentHpDisplay(int currentHpValue)
        {
            for (int i = 0; i < _halfHearts.Length; i++)
            {
                if (i < currentHpValue)
                    _halfHearts[i].SetActive(true);
                else
                    _halfHearts[i].SetActive(false);
            }
        }
        private void UpdateCurrentMaxHpDisplay(int currentMaxHpValue)
        {
            for (int i = 0; i < _backgroundsHeart.Length; i++)
            {
                if (i < currentMaxHpValue)
                    _backgroundsHeart[i].SetActive(true);
                else
                    _backgroundsHeart[i].SetActive(false);
            }
        }
    }
}
