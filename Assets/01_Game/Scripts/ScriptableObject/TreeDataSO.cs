using System.Numerics;
using UnityEngine;

namespace _01_Game.Scripts.ScriptableObject
{
    [CreateAssetMenu(fileName = "TreeDataSO", menuName = "Game/TreeData", order = 0)]
    public class TreeDataSO : UnityEngine.ScriptableObject
    {
        public string id;
        public string displayName;
        public string price;
        public string priceProduct;
        public Sprite sprite;
        public GameObject prefab;
    }
}