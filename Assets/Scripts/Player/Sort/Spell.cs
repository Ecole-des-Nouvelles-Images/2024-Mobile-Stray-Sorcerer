using UnityEngine;

namespace Player.Sort
{
    public class Spell : ScriptableObject
    {
        [SerializeField] private string name;
        [SerializeField] private GameObject prefab;
        [SerializeField] private bool pierce;
        [SerializeField] private bool bounce;
        [SerializeField] private bool explode;
        [SerializeField] private bool areaInvoker;
    }
}