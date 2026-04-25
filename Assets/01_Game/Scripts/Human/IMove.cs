using System;
using UnityEngine;

namespace _01_Game.Scripts.Human
{
    public interface IMove
    {
        public void MoveToPoint(Vector3 point);
        public void OnReachDestination(Action action);
    }
}