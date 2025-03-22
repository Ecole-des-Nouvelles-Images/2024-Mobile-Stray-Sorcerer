using System;

namespace Gameplay.GameData
{
    [Serializable]
    public class Snapshot
    {
        public float Time;
        public int MonsterLevel;
        public int Kill;
        public int MazeComplete;
        public int PlayerLevel;
        public int PlayerXp;
        public int PlayerMaxHp;
        public int PlayerHp;
        public int PlayerConstitution;
        public int PlayerSwiftness;
        public int PlayerPower;
        public int PlayerCurrentSpellIndex;
    }
}
