using _01_Game.Scripts.Manager;
using UnityEngine;

namespace _01_Game.Scripts.ScriptableObject
{
    [CreateAssetMenu(fileName = "ProfitUpgradeSO", menuName = "Game/ProfitUpgradeSO", order = 0)]
    public class ProfitUpgradeSO : UpgradeItemSO
    {
        [Header("UniqueData")]
        public int amount;
        public override void Apply()
        {
            base.Apply();
            SaveDataManager.Global.AddProfit(amount);
        }
    }
}