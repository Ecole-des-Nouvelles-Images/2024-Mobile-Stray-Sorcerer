using Player;
using UnityEngine;

namespace Gameplay
{
    public class DamageSource : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player")) other.gameObject.GetComponent<Character>().TakeDamage(10);
        }
    }
}