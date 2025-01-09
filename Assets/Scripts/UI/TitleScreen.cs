using System;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(Canvas))]
    public class TitleScreen: MonoBehaviour
    {
        private Canvas _canvas;
        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = GameObject.Find("Camera/CinemachineBrain").GetComponent<Camera>();

            if (!_mainCamera)
                throw new Exception("Title Screen setup: unable to find main camera");

            InitializeCanvas();
        }

        private void InitializeCanvas()
        {
            _canvas = GetComponent<Canvas>();

            _canvas.worldCamera = _mainCamera;
            _canvas.planeDistance = 1;
            _canvas.sortingLayerName = "UI";
            _canvas.sortingOrder = 0;
        }

        private void OnValidate()
        {
            if (!_canvas)
                InitializeCanvas();

            if (_canvas.renderMode != RenderMode.ScreenSpaceCamera)
            {
                Debug.LogWarning("Title Screen canvas render mode is locked to Screen Space Camera");
                _canvas.renderMode = RenderMode.ScreenSpaceCamera;
            }
        }
    }
}