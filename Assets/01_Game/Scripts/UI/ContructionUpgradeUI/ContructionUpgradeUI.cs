using System.Numerics;
using _01_Game.Scripts.Gameplay;
using _01_Game.Scripts.Manager;
using _01_Game.Scripts.ScriptableObject;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace _01_Game.Scripts.UI.ContructionUpgradeUI
{
    public class ContructionUpgradeUI : ScreenUI
    {
        [Header("UI")]
        [SerializeField] private RectTransform frameRect;
        [SerializeField] private Canvas canvas;
        [Header("Components")]
        [SerializeField] private TMP_Text lvTxt;
        [SerializeField] private TMP_Text displayText;
        [SerializeField] private TMP_Text priceText;
        [SerializeField] private TMP_Text priceProductTxt;
        [SerializeField] private Slider progressLv;
        [SerializeField] private GameObject btnUpgrade;
        [SerializeField] private GameObject maxObj;
        
        private Camera _camera;
        private Transform _target;
        private int _currentLevel = 1;
        private TreeDataSO _treeDataSo;
        private int _indexSlot;
        private void Awake()
        {
            _camera = Camera.main;
        }
        public override void OnShow()
        {
            base.OnShow();
            // Reset scale for popup
            frameRect.localScale = Vector3.zero;

            // Animate popup
            Tween.Scale(frameRect, Vector3.one, 0.3f, Ease.OutBack);
        }
        public void Setup(TreeDataSO treeDataSo ,Transform target)
        {
            if (target.TryGetComponent(out TreeSlot treeSlot))
            {
                _indexSlot = treeSlot.IndexSlot;
            }

            var data = SaveDataManager.Global.GetTreeData(_indexSlot);
            _currentLevel = data.level;
            _target =  target;
            _treeDataSo =  treeDataSo;
            SetupUI(treeDataSo);
            
            Vector2 screenPos = _camera.WorldToScreenPoint(target.position);
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
            lvTxt.text = $"Level {_currentLevel}";
            displayText.text = $"{treeDataSo.displayName}";
            priceText.text = $"{NumberFormatter.FormatShort(treeDataSo.PriceUpgrade(_currentLevel))}";
            progressLv.value = (float)_currentLevel / 10f;
            priceProductTxt.text = $"{NumberFormatter.FormatShort(treeDataSo.PriceProduct(_currentLevel))}";
            btnUpgrade.SetActive(_currentLevel < 10);
            maxObj.SetActive(_currentLevel >= 10);
        }

        public void UpgradeTree()
        {
            if(_currentLevel >= 10)
                return;
            _currentLevel++;
            SetupUI(_treeDataSo);
            SaveDataManager.Global.UpgradeTree(_indexSlot,_currentLevel);
        }
    }
}