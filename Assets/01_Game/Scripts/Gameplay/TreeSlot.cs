using _01_Game.Scripts.ScriptableObject;
using _01_Game.Scripts.UI;
using _01_Game.Scripts.UI.ConstructionBuildUI;
using UnityEngine;

namespace _01_Game.Scripts.Gameplay
{
    public enum TreeState
    {
        Box
    }
    public class TreeSlot : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private TreeDataSO treeData;
        
        private TreeState state = TreeState.Box;
        public void Interact()
        {
            switch (state)
            {
                case TreeState.Box:
                    MasterUI.Global.Get<ConstructionBuildUI>().Setup(treeData, transform.position);
                    MasterUI.Global.Show<ConstructionBuildUI>();
                    break;
            }
        }
        
    }
}