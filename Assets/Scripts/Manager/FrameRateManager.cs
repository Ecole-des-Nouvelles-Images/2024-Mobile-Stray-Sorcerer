using UnityEngine;

namespace Manager
{
    public class FrameRateManager : MonoBehaviour
    {
        [Header("Frame Settings")]

        [SerializeField] private int _targetFrameRate = 60;

        private void Awake()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = _targetFrameRate;
        }
    }
}
