using UnityEngine;

namespace Lighting
{
    [RequireComponent(typeof(SphereCollider))]
    public class EmitterManager : MonoBehaviour
    {
        public static float SwitchOnDuration = 1f;

        [SerializeField] private float _activationRadius = 30f;

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

            EmitterComposite container = other.GetComponent<EmitterComposite>();

            if (!container) return;

            container.IsActive = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("RuntimeDynamicEmitter")) return;

            EmitterComposite container = other.GetComponent<EmitterComposite>();

            if (!container) return;

            container.IsActive = false;
        }
    }
}