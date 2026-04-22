using System;
using _01_Game.Scripts.Manager;
using _01_Game.Scripts.ScriptableObject;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _01_Game.Scripts.UI.ConstructionBuildUI
{
    public class ConstructionBuildUI : ScreenUI
    {
        [Header("UI")]
        [SerializeField] private RectTransform frameRect;
        [SerializeField] private Canvas canvas;
        [Header("Components")]
        [SerializeField] private TMP_Text displayText;
        [SerializeField] private TMP_Text priceText;
        [SerializeField] private Image icon;

        private Camera _camera;
        private void Awake()
        {
            _camera = Camera.main;
        }

        public void Setup(TreeDataSO treeDataSo ,Vector3 position)
        {
            SetupUI(treeDataSo);
            Vector2 screenPos = _camera.WorldToScreenPoint(position);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                screenPos,
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
                out Vector2 localPos
            );
            frameRect.anchoredPosition = localPos;
        }

        private void SetupUI(TreeDataSO treeDataSo)
        {
            displayText.text = treeDataSo.displayName;
            priceText.text = NumberFormatter.FormatShort(treeDataSo.price);
            icon.sprite = treeDataSo.sprite;
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