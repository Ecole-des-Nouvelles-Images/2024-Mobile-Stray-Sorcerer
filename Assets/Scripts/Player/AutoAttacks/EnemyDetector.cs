using System.Collections.Generic;
using UnityEngine;

namespace Player.AutoAttacks
{
    public class EnemyDetector : MonoBehaviour
    {
        public List<GameObject> EnemiesInRange = new List<GameObject>();

        private void Update()
        {
            if (EnemiesInRange.Count != 0)
            {
                for (int i = 0; i < EnemiesInRange.Count; i++)
                {
                    if (EnemiesInRange[i].activeSelf == false)
                        EnemiesInRange.Remove(EnemiesInRange[i]);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                EnemiesInRange.Add(other.gameObject);
            }
            //DebugList();
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                for (int i = 0; i < EnemiesInRange.Count; i++)
                {
                    if(other.name == EnemiesInRange[i].name)return;
                }
                EnemiesInRange.Add(other.gameObject);
                //DebugList();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            EnemiesInRange.Remove(other.gameObject);
            //DebugList();
        }

        private void DebugList()
        {
            for (int i = 0; i < EnemiesInRange.Count; i++)
            {
                Debug.Log("Object nbr" + i + " name: " + EnemiesInRange[i].name);
            }
        }
    }
}
