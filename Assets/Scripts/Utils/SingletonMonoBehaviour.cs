using UnityEngine;

namespace Utils
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (!_instance)
                    _instance = (T)FindObjectOfType(typeof(T));

                return _instance;
            }
        }

        protected bool CheckInstance()
        {
            if (this == Instance) return true;
            Destroy(this);
            return false;
        }
    }
}