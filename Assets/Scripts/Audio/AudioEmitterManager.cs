using System;
using DG.Tweening;
using UnityEngine;

namespace Audio
{
    [RequireComponent(typeof(SphereCollider))]
    public class AudioEmitterManager: MonoBehaviour
    {
        [SerializeField] private float _activationRadius = 30f;
        // [SerializeField] private float _fadeDuration = 2f;
        
        private SphereCollider _collider;

        private void Awake()
        {
            _collider = GetComponent<SphereCollider>();
            _collider.radius = _activationRadius;

            if (!_collider.isTrigger)
                _collider.isTrigger = true;
        }

        private void OnValidate()
        {
            if (!_collider)
                _collider = GetComponent<SphereCollider>();

            _collider.radius = Mathf.Abs(_activationRadius);
            _collider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("RuntimeDynamicEmitter")) return;

            AudioSource source = other.GetComponentInChildren<AudioSource>();

            if (!source) return;

            source.Play();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("RuntimeDynamicEmitter")) return;

            AudioSource source = other.GetComponentInChildren<AudioSource>();

            if (!source) return;

            source.Stop();
        }
    }
}