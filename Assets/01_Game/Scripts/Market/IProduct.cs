using _01_Game.Scripts.Model;
using UnityEngine;

namespace _01_Game.Scripts.Market
{
    public interface IProduct
    {
        public ProductData GetProduct();
    }
}