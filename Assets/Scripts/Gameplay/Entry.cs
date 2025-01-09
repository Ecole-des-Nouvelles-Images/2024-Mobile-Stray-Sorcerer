using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay
{
    public class Entry : MonoBehaviour
    {
        [FormerlySerializedAs("_teleporteFX")]
        [Header("Animations Settings")]
        [SerializeField] private GameObject[] _dynamicFXs;

        private void Awake()
        {
            foreach (GameObject fx in _dynamicFXs) {
                fx.SetActive(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                foreach (GameObject fx in _dynamicFXs)
                {
                    fx.SetActive(false);
                }
            }
        }
    }
}