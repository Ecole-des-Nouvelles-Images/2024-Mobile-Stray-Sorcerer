using System;
using System.Collections;
using UnityEngine;

namespace Lighting
{
    [RequireComponent(typeof(SphereCollider))]
    public class AutoSwitchLight : MonoBehaviour
    {
        [Header("Detection")]
        [SerializeField] private float _playerDetectionRadius = 20f;

        [Header("Animation")]
        [SerializeField] private bool _useAnimation = true;
        [SerializeField] private float _switchDuration = 1.5f;

        public Action OnSwitchLightOn;

        private Light _light;
        private SphereCollider _collider;

        private void Awake()
        {
            _light = GetComponent<Light>();
            _collider = GetComponent<SphereCollider>();

            _light.enabled = false;
            _collider.radius = _playerDetectionRadius;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!_collider)
                _collider = GetComponent<SphereCollider>();
            _collider.radius = _playerDetectionRadius;
        }
#endif

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            if (_useAnimation)
                StartCoroutine(SwitchLight(true));
            else
                _light.enabled = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            if (_useAnimation)
                StartCoroutine(SwitchLight(false));
            else
                _light.enabled = false;
        }

        private IEnumerator SwitchLight(bool on)
        {
            float t = 0f;
            float initialIntensity = _light.intensity;

            if (on)
                _light.enabled = true;

            while (t < 1)
            {
                t += Time.deltaTime / _switchDuration;
                _light.intensity = Mathf.Lerp(initialIntensity, on ? 1 : 0, t);
                yield return null;
            }

            if (!on)
                _light.enabled = false;
            else
                OnSwitchLightOn?.Invoke();
        }
    }
}