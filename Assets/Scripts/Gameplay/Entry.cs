using System;
using System.Collections;
using DG.Tweening;
using Gameplay.GameData;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gameplay
{
    public class Entry : MonoBehaviour
    {
        [SerializeField] private float _fadeAnimDuration;
        [SerializeField] private GameObject[] _teleporteFX;
        [SerializeField] private GameObject _tutoMessage;
        [SerializeField] private TMP_Text _tutoTextMessage;

        private bool _triggerMessage;
        private float _timerAnim;

        private void Awake()
        {
            _tutoMessage.SetActive(false);
            foreach (GameObject fx in _teleporteFX)
            {
                fx.SetActive(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _tutoMessage.SetActive(true);
                StartCoroutine(Disactivation());
                foreach (GameObject fx in _teleporteFX)
                {
                    fx.SetActive(false);
                }

                _triggerMessage = true;
                _timerAnim = 0;
                _tutoTextMessage.DOFade(1, _fadeAnimDuration);
            }
        }

        private void Update()
        {
            if (_timerAnim < _fadeAnimDuration && _triggerMessage)
                _timerAnim += Time.deltaTime;
            else
            {
                _triggerMessage = false;
                _tutoTextMessage.DOFade(0, _fadeAnimDuration);
            }
        }

        private IEnumerator Disactivation()
        {
            yield return new WaitForSeconds(3f);
            _tutoMessage.SetActive(false);
        }
    }
}