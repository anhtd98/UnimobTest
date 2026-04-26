using System;
using System.Collections.Generic;
using System.Numerics;
using _01_Game.Scripts.Manager;
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
        private int _slot;
        private TreeData _treeData;
        public void Setup(TreeDataSO treeData, int indexSlot)
        {
            _treeDataSo = treeData;
            _slot = indexSlot;
            _treeData = SaveDataManager.Global.GetTreeData(_slot);
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
            p.price = _treeDataSo.PriceProduct(_treeData.level);
            Tween.Delay(1, SpawnProducts);
            return p;
        }

        #endregion
    }
}