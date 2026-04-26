using System;

namespace Gameplay.GameSaveDataSystem
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
