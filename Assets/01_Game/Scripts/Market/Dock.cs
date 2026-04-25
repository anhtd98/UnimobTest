using UnityEngine;

namespace _01_Game.Scripts.Market
{
    public enum DockStatus
    {
        Empty,
        Waiting,
        Ready,
    }
    public class Dock : MonoBehaviour
    {
        [SerializeField] private Transform deliveryPoint;
        [SerializeField] private Transform customerPoint;
        
        public DockStatus status;
        public Vector3 GetDockPosition()
        {
            return customerPoint.position;
        }
        public Vector3 GetDeliveryPosition()
        {
            return deliveryPoint.position;
        }
    }
}