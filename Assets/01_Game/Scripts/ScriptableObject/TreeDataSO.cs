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

        public BigInteger PriceUpgrade(int lv = 1)
        {
            return BigInteger.Parse(price) * (2 * lv);
        }
        public BigInteger PriceProduct(int lv = 1)
        {
            var a = BigInteger.Parse(priceProduct);
            return a + (a / 10 * (lv - 1));
        }
    }
}