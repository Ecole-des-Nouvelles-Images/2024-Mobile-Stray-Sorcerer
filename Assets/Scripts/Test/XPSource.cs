using Player;
using UnityEngine;

namespace Test
{
    public class XPSource : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            if(other.gameObject.CompareTag("Player"))other.gameObject.GetComponent<CharacterProperty>().GainExperience(10);
        }
    }
}
