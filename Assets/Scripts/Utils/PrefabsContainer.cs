using UnityEngine;

namespace Utils
{
    public class PrefabsContainer : SingletonMonoBehaviour<PrefabsContainer>
    {
        [Header("FieldBoost")] 
        [SerializeField] private GameObject _speedBoostPrefab;

        public GameObject SpeedBoostPrefab { get => _speedBoostPrefab; }
    }
}
