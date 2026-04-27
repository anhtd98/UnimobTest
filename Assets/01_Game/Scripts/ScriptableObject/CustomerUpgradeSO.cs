using _01_Game.Scripts.Manager;
using UnityEngine;

namespace _01_Game.Scripts.ScriptableObject
{
    [CreateAssetMenu(fileName = "CustomerUpgradeSO", menuName = "Game/CustomerUpgradeSO", order = 0)]
    public class CustomerUpgradeSO : UpgradeItemSO
    {
        [Header("UniqueData")]
        public int amount;
        public override void Apply()
        {
            base.Apply();
            SaveDataManager.Global.AddCustomer(amount);
        }
    }
}