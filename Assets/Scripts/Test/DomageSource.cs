using Player;
using UnityEngine;

namespace Test
{
    public class DomageSource : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            if(other.gameObject.CompareTag("Player"))other.gameObject.GetComponent<CharacterProperty>().TakeDamage(10);
        }
    }
}
