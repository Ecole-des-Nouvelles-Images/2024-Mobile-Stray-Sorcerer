using System;
using UnityEngine;
using UnityEngine.VFX;

namespace Lighting
{
    [Serializable]
    public class EffectEmitter
    {
        public enum Type { ParticleSystem, VFXGraph }

        public Type System;
        public ParticleSystem PS;
        public VisualEffect VFX;

        public Component Component
        {
            get
            {
                return System switch
                {
                    Type.ParticleSystem => PS,
                    Type.VFXGraph => VFX,
                    _ => throw new Exception($"Effect type {System} is not supported")
                };
            }
        }

        public EffectEmitter(ParticleSystem ps)
        {
            System = Type.ParticleSystem;
            PS = ps;
            VFX = null;
        }

        public EffectEmitter(VisualEffect vfx)
        {
            System = Type.VFXGraph;
            VFX = vfx;
            PS = null;
        }

        public override string ToString()
        {
            return Component.ToString();
        }

        public void Play()
        {
            switch (System)
            {
                case Type.ParticleSystem:
                    PS.Play();
                    break;
                case Type.VFXGraph:
                    VFX.Play();
                    break;
                default:
                    throw new Exception($"Effect type {System} is not supported");
            }
        }

        public void Stop()
        {
            if (PS)
                PS.Stop();

            if (VFX)
                VFX.Stop();
        }

        #region Implicit construction

        public static implicit operator EffectEmitter(ParticleSystem ps) {
            return new EffectEmitter(ps);
        }

        public static implicit operator EffectEmitter(VisualEffect vfx) {
            return new EffectEmitter(vfx);
        }

        #endregion

        #region Implicit reference

        public static implicit operator ParticleSystem(EffectEmitter effect) {
            return effect.PS;
        }

        public static implicit operator VisualEffect(EffectEmitter effect) {
            return effect.VFX;
        }

        #endregion
    }
}