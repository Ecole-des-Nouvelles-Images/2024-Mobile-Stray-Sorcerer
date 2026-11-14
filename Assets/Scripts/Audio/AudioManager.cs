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
            {
                GameObject player = GameObject.Find("Player");
                if (player != null)
                {
                    Transform ambientTransform = player.transform.Find("AmbientAudioSource");
                    if (ambientTransform != null)
                    {
                        _ambientSource = ambientTransform.GetComponent<AudioSource>();
                    }
                }
            }

            if (_ambientSource != null)
            {
                _ambientSource.Play();
                _ambientSource.DOFade(_ambientVolume, 1f).SetUpdate(true);
            }
            else
            {
                Debug.LogWarning("[AudioManager] Could not find 'AmbientAudioSource' as a child of 'Player'.");
            }
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
            float decibels = value <= 0f ? -80f : Mathf.Log10(value) * 20f;
            _mixer.SetFloat("AmbientVolume", decibels);
        }

        public void UpdateSFXVolume(float value)
        {
            float decibels = value <= 0f ? -80f : Mathf.Log10(value) * 20f;
            _mixer.SetFloat("SFXVolume", decibels);
        }

        public void MuteTemporarily(float muteDuration = 0.2f, float fadeDuration = 0.5f)
        {
            _mixer.SetFloat("MasterVolume", -80f);
            DOVirtual.DelayedCall(muteDuration, () =>
            {
                _mixer.DOSetFloat("MasterVolume", 0f, fadeDuration).SetUpdate(true);
            });
        }
    }
}
