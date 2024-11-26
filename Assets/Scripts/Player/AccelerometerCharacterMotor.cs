using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Player
{
    public class AccelerometerCharacterMotor : MonoBehaviour
    {
        [SerializeField] private SwitchControl switchControl;
        [SerializeField] private float xPower = 5000;
        [SerializeField] private float yPower = 5000;
        private Rigidbody _rb;

        private void Awake() {
            _rb = GetComponent<Rigidbody>();
        }

        public void MouvementA(InputAction.CallbackContext ctx) {
            //Debug.Log(ctx.ReadValue<Vector3>());
            if (switchControl.IsAccelerometer)
            {
                Vector3 vec = ctx.ReadValue<Vector3>();
                if(ctx.ReadValue<Vector3>().x > 0.15 || ctx.ReadValue<Vector3>().x < -0.15)
                    vec.x = Mathf.Clamp(vec.x * xPower,-500f,500f);
                if(ctx.ReadValue<Vector3>().y > 0.15 || ctx.ReadValue<Vector3>().y < -0.15)
                    vec.y = Mathf.Clamp(vec.y *yPower, -500f,500f);
                vec.z = 0;
                _rb.velocity = new Vector3(vec.y*-1,vec.z,vec.x) * Time.fixedDeltaTime;
            }

        }
    }
}
