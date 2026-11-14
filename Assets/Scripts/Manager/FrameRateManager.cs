using TMPro;
using UnityEngine;
using Utils;

namespace Manager
{
    public class FrameRateManager : SingletonMonoBehaviour<FrameRateManager>
    {
        [Header("Frame settings")]
        [SerializeField] private int _targetFrameRate = 60;

        [Header("Counter settings")]
        [SerializeField] private float _counterRefreshRate = 1;
        [SerializeField] private TMP_Text _counterBox;
        
        private float m_LastInterval;
        private int m_Frames;
        private string htmlColorTag;
        private const string fpsLabel = "{0:2}</color> <#8080ff>IPS \n<#FF8000>{1:2} <#8080ff>ms";
        
        public bool IsCounterEnabled => _counterBox.gameObject.activeSelf;
        
        private void Awake()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = _targetFrameRate;
        }
        
        private void Start()
        {
            m_LastInterval = Time.realtimeSinceStartup;
            m_Frames = 0;
        }

        public void EnableFPSCounter(bool enable)
        {
            _counterBox.gameObject.SetActive(enable);
        }

        private void Update()
        {
            if (!_counterBox.gameObject.activeSelf)
                return;
            
            m_Frames += 1;
            float timeNow = Time.realtimeSinceStartup;

            if (timeNow > m_LastInterval + _counterRefreshRate)
            {
                // display two fractional digits (f2 format)
                float fps = m_Frames / (timeNow - m_LastInterval);
                float ms = 1000.0f / Mathf.Max(fps, 0.00001f);

                if (fps < 30)
                    htmlColorTag = "<color=yellow>";
                else if (fps < 10)
                    htmlColorTag = "<color=red>";
                else
                    htmlColorTag = "<color=green>";

                _counterBox.SetText(htmlColorTag + fpsLabel, fps, ms);

                m_Frames = 0;
                m_LastInterval = timeNow;
            }
        }
    }
}
