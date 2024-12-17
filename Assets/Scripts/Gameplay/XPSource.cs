using Player;
using UnityEngine;

namespace Gameplay
{
    public class XPSource : MonoBehaviour
    {
        [SerializeField] private int _xpValue = 10;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Character.Instance.GainEXP(_xpValue);
                Destroy(gameObject);
            }
        }
    }
}