using System;
using System.Collections.Generic;
using System.Numerics;
using _01_Game.Scripts.Market;
using _01_Game.Scripts.Model;
using _01_Game.Scripts.ScriptableObject;
using MainGame.Services.Utils;
using PrimeTween;
using UnityEngine;

namespace _01_Game.Scripts.Gameplay
{
    public class ConstructionControl : MonoBehaviour, IProduct
    {
        [SerializeField] private List<Transform> productPoints;
        [SerializeField] private GameObject productPrefab;
        
        private TreeDataSO _treeDataSo;
        private List<Transform> products;
        
        public void Setup(TreeDataSO treeData)
        {
            _treeDataSo = treeData;
        }

        private void OnEnable()
        {
            SpawnProducts();
        }

        public void SpawnProducts()
        {
            if (productPoints == null || productPoints.Count == 0) return;

            Sequence seq = Sequence.Create();
            products = new();
            for (int i = 0; i < productPoints.Count; i++)
            {
                int index = i;

                seq.Group(Tween.Delay(index * 0.1f, () =>
                {
                    var prod = productPrefab.Spawn(productPoints[index]);
                    products.Add(prod.transform);
                }));
            }
        }
        #region Product

        public ProductData GetProduct()
        {
            var p = new ProductData();
            p.productsVisual = products;
            p.price = BigInteger.Parse(_treeDataSo.priceProduct);
            Tween.Delay(1, SpawnProducts);
            return p;
        }

        #endregion
    }
}