using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Player {
    public class AccelerometerCharacterMotor : MonoBehaviour {
        [SerializeField] private float speed = 500;
        [SerializeField] private Animator animatorImportedAnimation;
        
        private Rigidbody _rb;
        private Animator _animatorUnity;
        private float _currentInputYValue;
        private float _currentInputXValue;
        private void Awake() {
            _rb = GetComponent<Rigidbody>();
            _animatorUnity = GetComponent<Animator>();
        }

        public void MouvementA(InputAction.CallbackContext ctx) {
            if (ctx.ReadValue<Vector3>().x > 0.1 || ctx.ReadValue<Vector3>().x < -0.15) _currentInputXValue = ctx.ReadValue<Vector3>().x;
            else _currentInputXValue = 0;
            if (ctx.ReadValue<Vector3>().y > 0.15 || ctx.ReadValue<Vector3>().y < -0.15) _currentInputYValue = ctx.ReadValue<Vector3>().y;
            else _currentInputYValue = 0;
            _rb.velocity = new Vector3(_currentInputYValue*-1,0,_currentInputXValue).normalized 
                           * speed * Time.fixedDeltaTime;
            _animatorUnity.SetFloat(AnimatorParameterAccess.VelocityX,_rb.velocity.x);
            animatorImportedAnimation.SetFloat(AnimatorParameterAccess.VelocityX,_rb.velocity.x);
            _animatorUnity.SetFloat(AnimatorParameterAccess.VelocityY,_rb.velocity.z);
            animatorImportedAnimation.SetFloat(AnimatorParameterAccess.VelocityY,_rb.velocity.z);
        }
    }
}
