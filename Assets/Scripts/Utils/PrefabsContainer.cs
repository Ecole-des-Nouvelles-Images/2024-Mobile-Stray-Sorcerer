using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class PrefabsContainer : SingletonMonoBehaviour<PrefabsContainer>
{
    [Header("FieldBoost")] 
    [SerializeField] private GameObject _speedBoostPrefab;

    public GameObject SpeedBoostPrefab { get => _speedBoostPrefab; }
}
