using System;
using System.Collections.Generic;
using _01_Game.Scripts.Gameplay;
using _01_Game.Scripts.Market;
using _01_Game.Scripts.Model;
using _01_Game.Scripts.Tool;
using UnityEngine;

namespace _01_Game.Scripts.Human
{
    public class DeliveryControl : MonoBehaviour, IProduct
    {
        [SerializeField] private List<Transform> productPoints;
        
        private IMove _mover;
        private TreeSlot _treeSlot;
        private ProductData _productData;
        
        public TreeSlot TreeSlot => _treeSlot;
        private void Awake()
        {
            _mover = GetComponent<IMove>();
        }
        public void Setup(TreeSlot tree)
        {
            _treeSlot =  tree;
            MoveToTree();
        }
        private void MoveToTree()
        {
            _mover.MoveToPoint(_treeSlot.DeliveryPosition);
            _mover.OnReachDestination(GetProductFromTree);
        }
        public void MoveToPoint(Vector3 position, Action onComplete)
        {
            _mover.MoveToPoint(position);
            _mover.OnReachDestination(onComplete);
        }
        private void GetProductFromTree()
        {
            var product = _treeSlot.GetComponentInChildren<IProduct>();
            if (product != null)
            {
                _productData = product.GetProduct();
                for (int i = 0; i < _productData.productsVisual.Count; i++)
                {
                    _productData.productsVisual[i].SetParent(productPoints[i]);
                    _productData.productsVisual[i].position = productPoints[i].position;
                }
                Observer.Instance.Notify(ObserverKey.DeliveryReady, transform);
            }

        }

        public ProductData GetProduct()
        {
            return _productData;
        }
    }
}