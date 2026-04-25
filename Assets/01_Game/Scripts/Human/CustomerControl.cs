using System;
using _01_Game.Scripts.Market;
using MainGame.Services.Utils;
using UnityEngine;

namespace _01_Game.Scripts.Human
{
    public class CustomerControl : MonoBehaviour
    {
        private IMove _mover;
        private Vector3 _extractPosition;
        private Dock _dock;

        private void Awake()
        {
            _mover = GetComponent<IMove>();
        }

        public void Setup(Dock dock, Vector3 extractedPosition)
        {
            _extractPosition = extractedPosition;
            _dock =  dock;
            MoveToDock();
        }

        private void MoveToDock()
        {
            _mover.MoveToPoint(_dock.GetDockPosition());
            _mover.OnReachDestination(() => _dock.status = DockStatus.Ready);
        }

        private void MoveToExtract()
        {
            _mover.MoveToPoint(_extractPosition);
            _mover.OnReachDestination(() => this.Recycle());
        }
    }
}