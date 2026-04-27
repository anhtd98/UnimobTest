using _01_Game.Scripts.Manager;
using UnityEngine;

namespace _01_Game.Scripts.ScriptableObject
{
    [CreateAssetMenu(fileName = "UpgradeItem", menuName = "Game/UpgradeItem", order = 0)]
    public class UpgradeItemSO : UnityEngine.ScriptableObject
    {
        [Header("BaseData")]
        public string id;
        public string displayName;
        public string description;
        public string price;
        public Sprite icon;

        public virtual void Apply()
        {
            SaveDataManager.Global.AddUpgrade(id);
        }
    }
}