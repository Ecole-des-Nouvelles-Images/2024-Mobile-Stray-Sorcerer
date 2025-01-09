using System;
using UnityEngine;

namespace Lighting
{
    [Serializable]
    public class LightEmitter
    {
        public Light Light;

        public bool IsFlickering;
        public float FlickerDuration;
        public AnimationCurve FlickerCurve;

        private float _internalIntensity;

        public LightEmitter(Light light)
        {
            Light = light;
            _internalIntensity = light.intensity;
        }

        public void SwitchOn()
        {
            Light.enabled = true;

            /* if (IsFlickering)
            {
                Light.DOIntensity(_internalIntensity, EmitterManager.SwitchOnDuration).SetEase(Ease.InQuad).SetUpdate(true);
            }
            else
            {
                Light.DOIntensity(0, FlickerDuration).SetEase(FlickerCurve).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
            } */
        }

        public void SwitchOff()
        {
            Light.enabled = false;
            // Light.DOKill(true);
        }
    }
}