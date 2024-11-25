using UnityEngine;
using UnityEngine.InputSystem;

namespace Christopher.C_Scripts.Player
{
    public class JoystickCharacterMotor : MonoBehaviour
    {
        [SerializeField] private float xPower = 500;
        [SerializeField] private float yPower = 500;
        [SerializeField] private Animator animatorImportedAnimation;
        
        private Rigidbody _rb;
        private Animator _animatorUnity;
        private void Awake() {
            _rb = GetComponent<Rigidbody>();
            _animatorUnity = GetComponent<Animator>();
        }

        public void MouvementJ(InputAction.CallbackContext ctx) {
            Vector2 vec = ctx.ReadValue<Vector2>();
            if(ctx.ReadValue<Vector2>().x > 0.1 || ctx.ReadValue<Vector2>().x < -0.1)
                vec.x = Mathf.Clamp(vec.x * xPower,-500f,500f);
            if(ctx.ReadValue<Vector2>().y > 0.1 || ctx.ReadValue<Vector2>().y < -0.1)
                vec.y = Mathf.Clamp(vec.y *yPower, -500f,500f);
            _rb.velocity = new Vector3(vec.x,0,vec.y) * Time.fixedDeltaTime;
            _animatorUnity.SetFloat(AnimatorParameterAccess.VelocityX,_rb.velocity.x);
            animatorImportedAnimation.SetFloat(AnimatorParameterAccess.VelocityX,_rb.velocity.x);
            _animatorUnity.SetFloat(AnimatorParameterAccess.VelocityY,_rb.velocity.z);
            animatorImportedAnimation.SetFloat(AnimatorParameterAccess.VelocityY,_rb.velocity.z);
        }
    }
}
