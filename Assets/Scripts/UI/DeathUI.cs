using System;
using Gameplay.GameData;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class DeathUI : EndGameUI
    {
        [SerializeField] private GameObject _deathDisplay;
        private void OnEnable()
        {
            Character.OnPlayerDeath += SelfActivation;
        }

        private void OnDisable()
        {
            Character.OnPlayerDeath -= SelfActivation;
        }

        private void Start()
        {
            _deathDisplay.SetActive(false);
        }

        private void SelfActivation()
        {
            Time.timeScale = 0;
            _deathDisplay.SetActive(true);
            DataCollector.Instance.Death();
            UpdateDisplay();
        }

        public void ReturnTitle()
        {
            Time.timeScale = 1;
            DataCollector.Instance.ResetSave();
            SceneManager.LoadScene("Setup");
        }
    }
}