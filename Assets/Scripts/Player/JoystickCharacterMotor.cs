using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Player
{
    public class JoystickCharacterMotor : MonoBehaviour
    {
        [SerializeField] private float speed = 500f;
        [SerializeField] private Animator animatorImportedAnimation;
        
        private Rigidbody _rb;
        private Animator _animatorUnity;
        private Vector2 _joystickValue;
        private float _currentInputYValue;
        private float _currentInputXValue;
        private void Awake() {
            _rb = GetComponent<Rigidbody>();
            _animatorUnity = GetComponent<Animator>();
        }

        public void MouvementJ(InputAction.CallbackContext ctx) {
            _joystickValue = ctx.ReadValue<Vector2>().normalized;
            _rb.velocity = new Vector3(_joystickValue.x,0,_joystickValue.y) * speed * Time.fixedDeltaTime;
            _animatorUnity.SetFloat(AnimatorParameterAccess.VelocityX,_rb.velocity.x);
            animatorImportedAnimation.SetFloat(AnimatorParameterAccess.VelocityX,_rb.velocity.x);
            _animatorUnity.SetFloat(AnimatorParameterAccess.VelocityY,_rb.velocity.z);
            animatorImportedAnimation.SetFloat(AnimatorParameterAccess.VelocityY,_rb.velocity.z);
        }
    }
}
