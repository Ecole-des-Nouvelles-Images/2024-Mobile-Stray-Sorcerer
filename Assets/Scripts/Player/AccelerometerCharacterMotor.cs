using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Player {
    public class AccelerometerCharacterMotor : MonoBehaviour {
        [SerializeField] private float speed = 500;
        [SerializeField] private float sensibility = 0.01f;
        [SerializeField] private Animator animatorImportedAnimation;
        
        private Rigidbody _rb;
        private Animator _animatorUnity;
        private Vector3 _acceltrometerValue;
        private float _currentInputYValue;
        private float _currentInputXValue;
        private void Awake() {
            _rb = GetComponent<Rigidbody>();
            _animatorUnity = GetComponent<Animator>();
        }

        public void MouvementA(InputAction.CallbackContext ctx) {
            _acceltrometerValue = ctx.ReadValue<Vector3>().normalized;
             if (_acceltrometerValue.x > sensibility)
                 _currentInputXValue = _acceltrometerValue.x;
             if (_acceltrometerValue.x < -sensibility) 
                 _currentInputXValue = _acceltrometerValue.x;
             if (_acceltrometerValue.y > sensibility )
                 _currentInputYValue = _acceltrometerValue.y;
             if ( _acceltrometerValue.y < -sensibility)
                 _currentInputYValue = _acceltrometerValue.y;
           /*if(_acceltrometerValue.x < sensibility || _acceltrometerValue.x > -sensibility)
              _currentInputXValue = 0;
            if(_acceltrometerValue.y < sensibility || _acceltrometerValue.y > -sensibility)
                _currentInputYValue = 0;*/
            _rb.velocity = new Vector3(_currentInputYValue*-1,0,_currentInputXValue)*speed * Time.fixedDeltaTime;
            _animatorUnity.SetFloat(AnimatorParameterAccess.VelocityX,_rb.velocity.x);
            animatorImportedAnimation.SetFloat(AnimatorParameterAccess.VelocityX,_rb.velocity.x);
            _animatorUnity.SetFloat(AnimatorParameterAccess.VelocityY,_rb.velocity.z);
            animatorImportedAnimation.SetFloat(AnimatorParameterAccess.VelocityY,_rb.velocity.z);
        }
    }
}
