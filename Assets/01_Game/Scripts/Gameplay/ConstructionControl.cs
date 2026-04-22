using System;
using System.Collections.Generic;
using _01_Game.Scripts.ScriptableObject;
using MainGame.Services.Utils;
using PrimeTween;
using UnityEngine;

namespace _01_Game.Scripts.Gameplay
{
    public class ConstructionControl : MonoBehaviour
    {
        [SerializeField] private List<Transform> productPoints;
        [SerializeField] private GameObject productPrefab;
        
        private TreeDataSO _treeDataSo;
        
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

            for (int i = 0; i < productPoints.Count; i++)
            {
                int index = i;

                seq.Group(Tween.Delay(index * 0.1f, () =>
                {
                    var prod = productPrefab.Spawn(productPoints[index].position);
                }));
            }
        }
    }
}