using System.Collections.Generic;

namespace _01_Game.Scripts.Model
{
    [System.Serializable]
    public class GardenData
    {
        public List<TreeData> treeData;
        public int amountCustomer = 1;
    }

    [System.Serializable]
    public class TreeData
    {
        public int slot;
        public int level = 1;
        public TreeStatus status;
    }

    public enum TreeStatus
    {
        NotAvailable,
        Available,
    }
}