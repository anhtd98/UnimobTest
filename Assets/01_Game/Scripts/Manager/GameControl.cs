using System;
using _01_Game.Scripts.UI;
using _01_Game.Scripts.UI.MainUI;
using UnityEngine;

namespace _01_Game.Scripts.Manager
{
    public class GameControl : MonoBehaviour
    {
        public static GameControl Global;

        private void Awake()
        {
            Global = this;
        }

        private void Start()
        {
            MasterUI.Global.Show<MainUI>();
        }
    }
}