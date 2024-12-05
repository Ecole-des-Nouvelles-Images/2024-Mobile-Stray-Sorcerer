using Player;
using UnityEngine;

namespace Test
{
    public class XPSource : MonoBehaviour
    {
        [SerializeField]private int _xpValue = 10;
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
