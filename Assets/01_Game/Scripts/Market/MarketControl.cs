using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _01_Game.Scripts.Gameplay;
using _01_Game.Scripts.Human;
using _01_Game.Scripts.Manager;
using _01_Game.Scripts.Tool;
using MainGame.Services.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _01_Game.Scripts.Market
{
    public class MarketControl : MonoBehaviour
    {
        [SerializeField] private Transform customerStart;
        [SerializeField] private Transform customerEnd;
        [SerializeField] private Transform deliveryEnd;
        [SerializeField] private List<Dock> docks;
        [SerializeField] private GameObject customer;
        [SerializeField] private GameObject delivery;

        private int _customerCount = 1;
        
        private Queue _customerQueue = new Queue();
        private Queue _deliveryQueue = new Queue();

        private void Awake()
        {
            Observer.Instance.AddObserver(ObserverKey.RequestDelivery, OnRequestDelivery);
            Observer.Instance.AddObserver(ObserverKey.CustomerReady, CheckMarketStatus);
            Observer.Instance.AddObserver(ObserverKey.DeliveryReady, CheckMarketStatus);
        }

        private void Start()
        {
            SpawnCustomer();
        }

        private void OnDestroy()
        {
            Observer.Instance.RemoveObserver(ObserverKey.RequestDelivery, OnRequestDelivery);
            Observer.Instance.RemoveObserver(ObserverKey.CustomerReady, CheckMarketStatus);
            Observer.Instance.RemoveObserver(ObserverKey.DeliveryReady, CheckMarketStatus);
        }

        private void SpawnCustomer()
        {
            var cus = customer.Spawn(customerStart.position);
            if (cus.TryGetComponent(out CustomerControl customerMove))
            {
                var count = docks.Where(x => x.status == DockStatus.Empty).Count();
                var rd = Random.Range(0, count);
                docks[rd].status = DockStatus.Waiting;
                customerMove.Setup(docks[rd], customerEnd.position);
            }
        }

        private void OnRequestDelivery(object obj)
        {
            var tree =  obj as Transform;
            if (tree != null && tree.TryGetComponent(out TreeSlot treeSlot))
            {
                SpawnDelivery(treeSlot);
            }

        }
        private void SpawnDelivery(TreeSlot tree)
        {
            var cus = delivery.Spawn(deliveryEnd.position);
            if (cus.TryGetComponent(out DeliveryControl deliveryControl))
            {
                deliveryControl.Setup(tree);
            }
        }

        private void CheckMarketStatus(object obj)
        {
            var trans =  obj as Transform;
            if(trans == null) return;
            if (trans.TryGetComponent(out CustomerControl customerControl))
            {
                _customerQueue.Enqueue(customerControl);
            }
            if (trans.TryGetComponent(out DeliveryControl deliveryControl))
            {
                _deliveryQueue.Enqueue(deliveryControl);
            }

            if (_customerQueue.Count > 0 && _deliveryQueue.Count > 0)
            {
                var dControl = _deliveryQueue.Dequeue() as DeliveryControl;
                var cControl = _customerQueue.Dequeue() as CustomerControl;

                var dockTarget = cControl.Dock;

                dControl.MoveToPoint(dockTarget.GetDeliveryPosition(), () =>
                {
                    cControl.ReceiveProduct(dControl.GetProduct(), () =>
                    {
                        dControl.MoveToPoint(deliveryEnd.position, () => dControl.Recycle());
                        cControl.MoveToPoint(customerEnd.position, () => cControl.Recycle());
                        dockTarget.status = DockStatus.Empty;
                        SpawnCustomer();
                        SpawnDelivery(dControl.TreeSlot);

                        var money = dControl.GetProduct().price;
                        SaveDataManager.Global.AddGold(money);
                    });
                });
            }
        }
    }
}