using System;
using System.Collections.Generic;
using System.Linq;
using _01_Game.Scripts.Human;
using MainGame.Services.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _01_Game.Scripts.Market
{
    public class MarketControl : MonoBehaviour
    {
        [SerializeField] private Transform customerStart;
        [SerializeField] private Transform customerEnd;
        [SerializeField] private List<Dock> docks;
        [SerializeField] private GameObject customer;

        private int _customerCount = 1;

        private void Awake()
        {
            
        }

        private void Start()
        {
            SpawnCustomer();
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
    }
}