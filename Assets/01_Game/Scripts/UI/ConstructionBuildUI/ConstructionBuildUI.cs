using System;
using PrimeTween;
using UnityEngine;

namespace _01_Game.Scripts.UI.ConstructionBuildUI
{
    public class ConstructionBuildUI : ScreenUI
    {
        [Header("UI")]
        [SerializeField] private RectTransform frameRect;
        [SerializeField] private Canvas canvas;

        private Camera _camera;
        private void Awake()
        {
            _camera = Camera.main;
        }

        public void Setup(Vector3 position)
        {
            Vector2 screenPos = _camera.WorldToScreenPoint(position);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                screenPos,
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
                out Vector2 localPos
            );

            frameRect.anchoredPosition = localPos;
        }

        public override void OnShow()
        {
            base.OnShow();
            // Reset scale for popup
            frameRect.localScale = Vector3.zero;

            // Animate popup
            Tween.Scale(frameRect, Vector3.one, 0.3f, Ease.OutBack);
        }
    }
}