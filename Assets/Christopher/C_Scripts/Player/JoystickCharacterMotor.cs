using UnityEngine;
using UnityEngine.InputSystem;

namespace Christopher.C_Scripts.Player
{
    public class JoystickCharacterMotor : MonoBehaviour
    {
        [SerializeField] private float speed = 500f;
        [SerializeField] private Animator animatorImportedAnimation;
        
        private Rigidbody _rb;
        private Animator _animatorUnity;
        private float _currentInputYValue;
        private float _currentInputXValue;
        private void Awake() {
            _rb = GetComponent<Rigidbody>();
            _animatorUnity = GetComponent<Animator>();
        }

        public void MouvementJ(InputAction.CallbackContext ctx) {
            if (ctx.ReadValue<Vector2>().x > 0.15 || ctx.ReadValue<Vector2>().x < -0.15) 
                _currentInputXValue = ctx.ReadValue<Vector2>().x;
            else _currentInputXValue = 0;
            if (ctx.ReadValue<Vector2>().y > 0.15 || ctx.ReadValue<Vector2>().y < -0.15) 
                _currentInputYValue = ctx.ReadValue<Vector2>().y;
            else _currentInputYValue = 0;
            _rb.velocity = new Vector3(ctx.ReadValue<Vector2>().x,0,ctx.ReadValue<Vector2>().y).normalized * speed 
                * Time.fixedDeltaTime;
            _animatorUnity.SetFloat(AnimatorParameterAccess.VelocityX,_rb.velocity.x);
            animatorImportedAnimation.SetFloat(AnimatorParameterAccess.VelocityX,_rb.velocity.x);
            _animatorUnity.SetFloat(AnimatorParameterAccess.VelocityY,_rb.velocity.z);
            animatorImportedAnimation.SetFloat(AnimatorParameterAccess.VelocityY,_rb.velocity.z);
        }
    }
}
