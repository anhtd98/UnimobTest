using System;
using System.Collections.Generic;
using _01_Game.Scripts.Market;
using _01_Game.Scripts.Model;
using _01_Game.Scripts.Tool;
using MainGame.Services.Utils;
using PrimeTween;
using UnityEngine;

namespace _01_Game.Scripts.Human
{
    public class CustomerControl : MonoBehaviour
    {
        [SerializeField] private List<Transform> productPoints;
        
        private IMove _mover;
        private Vector3 _extractPosition;
        private Dock _dock;
        
        public Dock Dock => _dock;

        private void Awake()
        {
            _mover = GetComponent<IMove>();
        }

        public void Setup(Dock dock, Vector3 extractedPosition)
        {
            _extractPosition = extractedPosition;
            _dock =  dock;
            MoveToDock();
        }

        private void MoveToDock()
        {
            _mover.MoveToPoint(_dock.GetDockPosition());
            _mover.OnReachDestination(() =>
            {
                _dock.status = DockStatus.Ready;
                Observer.Instance.Notify(ObserverKey.CustomerReady, transform);
            });
        }
        public void MoveToPoint(Vector3 position, Action onComplete)
        {
            _mover.MoveToPoint(position);
            _mover.OnReachDestination(onComplete);
        }
        private void MoveToExtract()
        {
            _mover.MoveToPoint(_extractPosition);
            _mover.OnReachDestination(() => this.Recycle());
        }

        public void ReceiveProduct(ProductData product, Action onComplete)
        {
            for (int i = 0; i < product.productsVisual.Count; i++)
            {
                var item = product.productsVisual[i];
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

            Tween.Delay(0.3f,() => onComplete());

        }
    }
}