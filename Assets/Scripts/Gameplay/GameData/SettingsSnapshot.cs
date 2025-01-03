using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.GameData
{
    [Serializable]
    public class SettingsSnapshot
    {
        public bool IsLeftJoystick;
        public float MusicSlider;
        public float SfxSlider;
        public float LuminositySlider;
    }
}
