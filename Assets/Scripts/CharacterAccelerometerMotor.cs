using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterAccelerometerMotor : MonoBehaviour {
    [SerializeField] private float _xPower = 100;
    [SerializeField] private float _yPower = 100;
    private Rigidbody _rb;

    private void Awake() {
        _rb = GetComponent<Rigidbody>();
    }

    public void Mouvement(InputAction.CallbackContext ctx) {
        //Debug.Log(ctx.ReadValue<Vector3>());

        Vector3 vec = ctx.ReadValue<Vector3>();
        vec.x = vec.x * _xPower;
        vec.y = vec.y *_yPower;
        vec.z = 0;
        _rb.velocity = new Vector3(vec.x,vec.z,vec.y) * Time.fixedDeltaTime;
    }
}
