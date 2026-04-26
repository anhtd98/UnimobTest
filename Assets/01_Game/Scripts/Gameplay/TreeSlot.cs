using System;
using _01_Game.Scripts.Manager;
using _01_Game.Scripts.Market;
using _01_Game.Scripts.Model;
using _01_Game.Scripts.ScriptableObject;
using _01_Game.Scripts.Tool;
using _01_Game.Scripts.UI;
using _01_Game.Scripts.UI.ConstructionBuildUI;
using _01_Game.Scripts.UI.ContructionUpgradeUI;
using MainGame.Services.Utils;
using PrimeTween;
using UnityEngine;

namespace _01_Game.Scripts.Gameplay
{
    public enum TreeState
    {
        Box,
        Unboxing,
        Tree
    }
    public class TreeSlot : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private TreeDataSO treeData;

        [Header("View")] 
        [SerializeField] private Animation boxAnim;
        [SerializeField] private GameObject boxObject;
        [SerializeField] private Transform deliveryPosition;
        
        private TreeState _state = TreeState.Box;
        private GameObject _construction;
        private int _indexSlot = 1;
        private TreeData _treeData;
        
        public Vector3 DeliveryPosition => deliveryPosition.position;
        public int IndexSlot => _indexSlot;

        private void Start()
        {
            Observer.Instance.AddObserver(ObserverKey.OpenBox,OpenBox);
        }

        private void OnDestroy()
        {
            Observer.Instance.RemoveObserver(ObserverKey.OpenBox,OpenBox);
        }

        public void Setup(int indexSlot)
        {
            _indexSlot = indexSlot;
            _treeData = SaveDataManager.Global.GetTreeData(indexSlot);
            if (_treeData.status == TreeStatus.Available)
            {
                _state = TreeState.Tree;
                SetupVisual();
            }
        }
        public void Interact()
        {
            switch (_state)
            {
                case TreeState.Box:
                    MasterUI.Global.Get<ConstructionBuildUI>().Setup(treeData, transform);
                    MasterUI.Global.Show<ConstructionBuildUI>();
                    break;
                case TreeState.Unboxing:
                    break;
                case TreeState.Tree:
                    MasterUI.Global.Get<ContructionUpgradeUI>().Setup(treeData, transform);
                    MasterUI.Global.Show<ContructionUpgradeUI>();
                    break;
            }
        }

        public void OpenBox(object data)
        {
            if (transform == data as Transform)
            {
                _state = TreeState.Unboxing;
                boxAnim.Play("BoxOpen");
                Tween.Delay(10, () =>
                {
                    SetupVisual();
                });
            }
        }

        private void SetupVisual()
        {
            boxObject.SetActive(false);
            _construction = treeData.prefab.Spawn(transform.position + Vector3.up*0.5f, Quaternion.identity);
            _construction.transform.SetParent(transform);
            _construction.GetComponent<ConstructionControl>().Setup(treeData);
            _state = TreeState.Tree;
            Observer.Instance.Notify(ObserverKey.RequestDelivery, transform);
        }
    }
}