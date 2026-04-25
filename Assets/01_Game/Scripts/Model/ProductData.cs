using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

namespace _01_Game.Scripts.Model
{
    [Serializable]
    public class ProductData
    {
        public List<Transform> productsVisual;
        public BigInteger price;
    }
}