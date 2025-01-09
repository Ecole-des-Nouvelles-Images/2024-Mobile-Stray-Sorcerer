using System.Collections.Generic;
using UnityEngine;

namespace Lighting
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class EmitterComposite: MonoBehaviour
    {
        [SerializeField] private List<LightEmitter> _lights;
        [SerializeField] private List<EffectEmitter> _emitters;

        private bool _isActive;

        public bool IsActive
        {
            get => _isActive;
            set {
                _isActive = value;
                if (_isActive)
                    EnableAll();
                else
                    DisableAll();
            }
        }

        private void Start()
        {
            IsActive = false;
        }

        public void DisableAll()
        {
            foreach (EffectEmitter t in _emitters)
                t.Stop();

            foreach (LightEmitter t in _lights)
            {
                t.SwitchOff();
            }
        }

        public void EnableAll()
        {
            foreach (EffectEmitter t in _emitters)
                t.Play();

            foreach (LightEmitter t in _lights)
            {
                t.SwitchOn();
            }
        }
    }
}