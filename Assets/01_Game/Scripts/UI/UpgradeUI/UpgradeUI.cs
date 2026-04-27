using System.Collections.Generic;
using System.Linq;
using _01_Game.Scripts.Manager;
using _01_Game.Scripts.ScriptableObject;
using PrimeTween;
using UnityEngine;

namespace _01_Game.Scripts.UI.UpgradeUI
{
    public class UpgradeUI : ScreenUI
    {
        [Header("Data")] 
        [SerializeField] private List<UpgradeItemSO> allUpgrade;
        [Header("UI")] 
        [SerializeField] private RectTransform frameRect;
        [SerializeField] private Transform content;
        [SerializeField] private UpgradeItemView upgradeItemView;

        public override void OnShow()
        {
            base.OnShow();
            frameRect.localScale = Vector3.zero;
            Tween.Scale(frameRect, Vector3.one, 0.3f, Ease.OutBack);
            SetupUI();
        }

        private void SetupUI()
        {
            var storeUpgrade = SaveDataManager.Global.GetAllUpgrade();
            var availableUpgrade = allUpgrade.Where(x => !storeUpgrade.Contains(x.id)).ToList();
            for (int i = 0; i < availableUpgrade.Count; i++)
            {
                var upgrade = Instantiate(upgradeItemView, content);
                upgrade.Setup(availableUpgrade[i]);
            }
        }
    }
}