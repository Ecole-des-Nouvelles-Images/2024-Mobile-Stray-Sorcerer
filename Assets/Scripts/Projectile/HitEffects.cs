using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffects : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (!other.transform.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

}
