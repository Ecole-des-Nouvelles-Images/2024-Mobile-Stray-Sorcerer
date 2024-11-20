using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class CharacterAccelerometerMotor : MonoBehaviour
{
    [SerializeField] private float xPower = 100;
    [SerializeField] private float yPower = 100;
    private Rigidbody _rb;

    private void Awake() {
        _rb = GetComponent<Rigidbody>();
    }

    public void Mouvement(InputAction.CallbackContext ctx) {
        //Debug.Log(ctx.ReadValue<Vector3>());

        Vector3 vec = ctx.ReadValue<Vector3>();
        if(ctx.ReadValue<Vector3>().x > 0.1 || ctx.ReadValue<Vector3>().x < 0.1)
            vec.x = Mathf.Clamp(vec.x * xPower,-500f,500f);
        if(ctx.ReadValue<Vector3>().y > 0.1 || ctx.ReadValue<Vector3>().y < 0.1)
            vec.y = Mathf.Clamp(vec.y *yPower, -500f,500f);
        vec.z = 0;
        _rb.velocity = new Vector3(vec.y,vec.z,vec.x*-1) * Time.fixedDeltaTime;
        Debug.Log(ctx.ReadValue<Vector3>());
       /* Vector3 vec = new Vector3();
        if (ctx.ReadValue<Vector3>().x > SensorSensibility)
        {
            vec.z = xPower;
        }
        else if (ctx.ReadValue<Vector3>().x > -SensorSensibility)
        {
            vec.z = -xPower;
        }
        else
        {
            vec.z = 0;
        }

        if (ctx.ReadValue<Vector3>().y > SensorSensibility)
        {
            vec.x = yPower;
        }
        else if (ctx.ReadValue<Vector3>().y < -SensorSensibility)
        {
            vec.x = -yPower;
        }
        else
        {
            vec.x = 0;
        }
        _rb.velocity = vec * Time.fixedDeltaTime;*/
    }
}
