using DG.Tweening;
using Manager;
using UnityEngine;
using UnityEngine.Audio;
using Utils;

namespace Audio
{
    public class AudioManager : SingletonMonoBehaviour<AudioManager>
    {
        [Header("Mixer system")]
        [SerializeField] private AudioMixer _mixer;

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
            if (!_ambientSource)
                _ambientSource = GameObject.Find("Player/AmbientAudioSource").GetComponent<AudioSource>();

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

        public void UpdateMusicVolume(float value)
        {
            _mixer.SetFloat("AmbientVolume", value);
        }

        public void UpdateSFXVolume(float value)
        {
            return;
        }
    }
}
