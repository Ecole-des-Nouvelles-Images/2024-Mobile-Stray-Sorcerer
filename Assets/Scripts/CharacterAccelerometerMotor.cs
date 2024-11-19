using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterAccelerometerMotor : MonoBehaviour {
    [SerializeField] private float _xPower = 30;
    [SerializeField] private float _yPower = 100;
    private Rigidbody2D _rb;

    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Mouvement(InputAction.CallbackContext ctx) {
        //Debug.Log(ctx.ReadValue<Vector3>());

        Vector3 vec = ctx.ReadValue<Vector3>();
        vec.x = vec.x * _xPower;
        vec.z = 0;
        vec.y = vec.y *_yPower;
        _rb.velocity = vec * Time.fixedDeltaTime;
    }
}
