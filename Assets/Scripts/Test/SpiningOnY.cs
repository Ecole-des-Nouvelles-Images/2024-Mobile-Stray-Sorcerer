using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiningOnY : MonoBehaviour
{
    [SerializeField] private int _speed;
    void Update()
    {
        Spining();
    }

    private void Spining()
    {
        if (transform.rotation.y < 0)
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        if (transform.rotation.y > 360)
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        transform.Rotate(0,(transform.rotation.y+_speed)*Time.deltaTime,0);
    }
}
