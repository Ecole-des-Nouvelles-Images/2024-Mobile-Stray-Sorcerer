using UnityEngine;
using TMPro;

namespace Utils
{
    public class TMPFrameRateCounter : MonoBehaviour
    {
        public float UpdateInterval = 5.0f;
        private float _mLastInterval = 0;
        private int _mFrames = 0;

        public enum FpsCounterAnchorPositions { TopLeft, BottomLeft, TopRight, BottomRight };

        public FpsCounterAnchorPositions AnchorPosition = FpsCounterAnchorPositions.TopLeft;

        private string _htmlColorTag;
        private const string _FPS_LABEL = "{0:2}</color> <#8080ff>FPS \n<#FF8000>{1:2} <#8080ff>MS";

        private TextMeshPro _mTextMeshPro;
        private Transform _mFrameCounterTransform;
        private Camera _mCamera;

        private FpsCounterAnchorPositions _lastAnchorPosition;

        public Vector2 Offset;

        public static GameObject FrameCounter;

        void Awake()
        {
            if (!enabled)
                return;

            _mCamera = GameObject.Find("Camera/CinemachineBrain").GetComponent<Camera>();
            // Application.targetFrameRate = 9999;
            Debug.Log("TMPFrameCounter camera is : " + (_mCamera ? "OK" : "null"));

            FrameCounter = new GameObject("Frame Counter");
            FrameCounter.transform.parent = _mCamera.transform;

            _mTextMeshPro = FrameCounter.AddComponent<TextMeshPro>();
            _mTextMeshPro.font = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
            _mTextMeshPro.fontSharedMaterial = Resources.Load<Material>("Fonts & Materials/LiberationSans SDF - Overlay");

            _mFrameCounterTransform = FrameCounter.transform;
            _mFrameCounterTransform.SetParent(_mCamera.transform);
            _mFrameCounterTransform.localRotation = Quaternion.identity;

            _mTextMeshPro.textWrappingMode = TextWrappingModes.NoWrap;
            _mTextMeshPro.fontSize = 24;
            //m_TextMeshPro.FontColor = new Color32(255, 255, 255, 128);
            //m_TextMeshPro.edgeWidth = .15f;
            //m_TextMeshPro.isOverlay = true;

            //m_TextMeshPro.FaceColor = new Color32(255, 128, 0, 0);
            //m_TextMeshPro.EdgeColor = new Color32(0, 255, 0, 255);
            //m_TextMeshPro.FontMaterial.renderQueue = 4000;

            //m_TextMeshPro.CreateSoftShadowClone(new Vector2(1f, -1f));

            Set_FrameCounter_Position(AnchorPosition);
            _lastAnchorPosition = AnchorPosition;
        }

        void Start()
        {
            if (!_mCamera)
            {
                _mCamera = GameObject.Find("Camera/CinemachineBrain").GetComponent<Camera>();
                FrameCounter.transform.SetParent(_mCamera.transform);
            }
            _mLastInterval = Time.realtimeSinceStartup;
            _mFrames = 0;
            FrameCounter.SetActive(false);
        }

        void Update()
        {
            if (AnchorPosition != _lastAnchorPosition)
                Set_FrameCounter_Position(AnchorPosition);

            _lastAnchorPosition = AnchorPosition;

            _mFrames += 1;
            float timeNow = Time.realtimeSinceStartup;

            if (timeNow > _mLastInterval + UpdateInterval)
            {
                // display two fractional digits (f2 format)
                float fps = _mFrames / (timeNow - _mLastInterval);
                float ms = 1000.0f / Mathf.Max(fps, 0.00001f);

                if (fps < 30)
                    _htmlColorTag = "<color=yellow>";
                else if (fps < 10)
                    _htmlColorTag = "<color=red>";
                else
                    _htmlColorTag = "<color=green>";

                //string format = System.String.Format(htmlColorTag + "{0:F2} </color>FPS \n{1:F2} <#8080ff>MS",fps, ms);
                //m_TextMeshPro.text = format;

                _mTextMeshPro.SetText(_htmlColorTag + _FPS_LABEL, fps, ms);

                _mFrames = 0;
                _mLastInterval = timeNow;
            }
        }

        void Set_FrameCounter_Position(FpsCounterAnchorPositions anchorPosition)
        {
            //Debug.Log("Changing frame counter anchor position.");
            _mTextMeshPro.margin = new Vector4(1f, 1f, 1f, 1f);

            if (!_mCamera)
                _mCamera = GameObject.Find("Camera/CinemachineBrain").GetComponent<Camera>();

            switch (anchorPosition)
            {
                case FpsCounterAnchorPositions.TopLeft:
                    _mTextMeshPro.alignment = TextAlignmentOptions.TopLeft;
                    _mTextMeshPro.rectTransform.pivot = new Vector2(0, 1);
                    _mFrameCounterTransform.position = _mCamera.ViewportToWorldPoint(new Vector3(0 + Offset.x, 1 + Offset.y, 100.0f));
                    break;
                case FpsCounterAnchorPositions.BottomLeft:
                    _mTextMeshPro.alignment = TextAlignmentOptions.BottomLeft;
                    _mTextMeshPro.rectTransform.pivot = new Vector2(0, 0);
                    _mFrameCounterTransform.position = _mCamera.ViewportToWorldPoint(new Vector3(0 + Offset.x, 0 + Offset.y, 100.0f));
                    break;
                case FpsCounterAnchorPositions.TopRight:
                    _mTextMeshPro.alignment = TextAlignmentOptions.TopRight;
                    _mTextMeshPro.rectTransform.pivot = new Vector2(1, 1);
                    _mFrameCounterTransform.position = _mCamera.ViewportToWorldPoint(new Vector3(1 + Offset.x, 1 + Offset.y, 100.0f));
                    break;
                case FpsCounterAnchorPositions.BottomRight:
                    _mTextMeshPro.alignment = TextAlignmentOptions.BottomRight;
                    _mTextMeshPro.rectTransform.pivot = new Vector2(1, 0);
                    _mFrameCounterTransform.position = _mCamera.ViewportToWorldPoint(new Vector3(1+ Offset.x, 0 + Offset.y, 100.0f));
                    break;
            }
        }

        public static void ToggleFrameCounter()
        {

        }
    }
}
