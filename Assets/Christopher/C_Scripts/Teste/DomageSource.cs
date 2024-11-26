using System;
using System.Collections;
using System.Collections.Generic;
using Christopher.C_Scripts.Player;
using UnityEngine;

public class DomageSource : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Player"))other.gameObject.GetComponent<CharacterProperty>().TakeDamage(10);
    }
}
