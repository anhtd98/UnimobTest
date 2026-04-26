using System;
using System.Collections.Generic;
using _01_Game.Scripts.Gameplay;
using UnityEngine;

namespace _01_Game.Scripts.Market
{
    public class GardenControl : MonoBehaviour
    {
        public List<TreeSlot> treeSlots;

        private void Start()
        {
            for (int i = 0; i < treeSlots.Count; i++)
            {
                treeSlots[i].Setup(i + 1);
            }
        }
    }
}