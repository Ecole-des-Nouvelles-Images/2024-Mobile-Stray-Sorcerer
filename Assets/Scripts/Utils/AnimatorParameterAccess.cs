using UnityEngine;

namespace Utils
{
    public static class AnimatorParameterAccess
    {
        //from CharacterUnity && CharacterImportedAnimation
        public static readonly int VelocityX = Animator.StringToHash("VelocityX");
        public static readonly int VelocityY = Animator.StringToHash("VelocityY");
    }
}
