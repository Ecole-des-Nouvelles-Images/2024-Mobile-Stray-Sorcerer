using UnityEngine;
using UnityEngine.InputSystem;

namespace Christopher.C_Scripts.Player {
    public class AccelerometerCharacterMotor : MonoBehaviour {
        [SerializeField] private float xPower = 5000;
        [SerializeField] private float yPower = 5000;
        [SerializeField] private Animator animatorImportedAnimation;
        
        private Rigidbody _rb;
        private Animator _animatorUnity;
        private void Awake() {
            _rb = GetComponent<Rigidbody>();
            _animatorUnity = GetComponent<Animator>();
        }

        public void MouvementA(InputAction.CallbackContext ctx) {
            Vector3 vec = ctx.ReadValue<Vector3>();
            if(ctx.ReadValue<Vector3>().x > 0.15 || ctx.ReadValue<Vector3>().x < -0.15)
                vec.x = Mathf.Clamp(vec.x * xPower,-500f,500f);
            if(ctx.ReadValue<Vector3>().y > 0.15 || ctx.ReadValue<Vector3>().y < -0.15)
                vec.y = Mathf.Clamp(vec.y *yPower, -500f,500f);
            vec.z = 0;
            _rb.velocity = new Vector3(vec.y*-1,vec.z,vec.x) * Time.fixedDeltaTime;
            _animatorUnity.SetFloat(AnimatorParameterAccess.VelocityX,_rb.velocity.x);
            animatorImportedAnimation.SetFloat(AnimatorParameterAccess.VelocityX,_rb.velocity.x);
            _animatorUnity.SetFloat(AnimatorParameterAccess.VelocityY,_rb.velocity.z);
            animatorImportedAnimation.SetFloat(AnimatorParameterAccess.VelocityY,_rb.velocity.z);
        }
    }
}
