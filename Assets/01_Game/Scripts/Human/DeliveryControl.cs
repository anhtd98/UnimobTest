using System;
using System.Collections.Generic;
using _01_Game.Scripts.Gameplay;
using _01_Game.Scripts.Market;
using _01_Game.Scripts.Model;
using _01_Game.Scripts.Tool;
using PrimeTween;
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
                    var item = _productData.productsVisual[i];
                    var target = productPoints[i];

                    Vector3 start = item.position;
                    Vector3 end = target.position;

                    float duration = 0.3f;
                    float height = 1.5f;

                    item.SetParent(target, true);

                    float delay = i * 0.05f; // lệch 0.05s mỗi item

                    Tween.Delay(delay).OnComplete(() =>
                    {
                        Tween.Custom(0f, 1f, duration, t =>
                        {
                            Vector3 pos = Vector3.Lerp(start, end, t);
                            float arc = height * 4f * t * (1f - t);
                            pos.y += arc;

                            item.position = pos;
                        });
                    });
                }

                Tween.Delay(0.3f, () =>
                {
                    Observer.Instance.Notify(ObserverKey.DeliveryReady, transform);
                });


            }

        }

        public ProductData GetProduct()
        {
            return _productData;
        }
    }
}