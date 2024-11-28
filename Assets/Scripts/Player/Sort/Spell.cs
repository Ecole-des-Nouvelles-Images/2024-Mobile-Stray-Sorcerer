using UnityEngine;

namespace Player.Sort
{
    [CreateAssetMenu(fileName = "New Spell", menuName = "SO/Spell")]
    public class Spell : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private bool _pierce;
        [SerializeField] private bool _bounce;
        [SerializeField] private bool _explode;
        [SerializeField] private bool _areaInvoker;
    }
}