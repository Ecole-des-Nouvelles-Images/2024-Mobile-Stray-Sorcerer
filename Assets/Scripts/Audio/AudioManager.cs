using System;
using DG.Tweening;
using Manager;
using UnityEngine;
using Utils;

namespace Audio
{
    public class AudioManager : SingletonMonoBehaviour<AudioManager>
    {
        [Header("Ambient")]
        [SerializeField] private AudioSource _ambientSource;
        [SerializeField] [Range(0, 1)] private float _ambientVolume = 0.5f;

        private void OnEnable()
        {
            GameManager.OnGameStart += PlayAmbient;
        }

        private void OnDisable()
        {
            GameManager.OnGameStart -= PlayAmbient;
        }

        private void PlayAmbient()
        {
            _ambientSource.Play();
            _ambientSource.DOFade(_ambientVolume, 1f).SetUpdate(true);
        }

        private void DisableAmbient()
        {
            _ambientSource.DOFade(0, 1f).SetUpdate(true).OnComplete(() =>
            {
                _ambientSource.Stop();
            });

        }
    }
}
