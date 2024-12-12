using UnityEngine;

namespace Player.Spells_Effects
{
    [CreateAssetMenu(fileName = "New Spell", menuName = "SO/Spell")]
    public class Spell : ScriptableObject
    {
        public string Name;
        public AudioClip ThrowingSound;
        public GameObject ProjectilePrefab;
        public AudioClip ImpactSound;
        public GameObject ImpactPrefab;
        public int Damage;
        public bool Pierce;
        public int PierceValue;
        public bool Bounce;
        public int BounceValue;
        public bool AreaInvoker;
        public GameObject ZonePrefab;
    }
}