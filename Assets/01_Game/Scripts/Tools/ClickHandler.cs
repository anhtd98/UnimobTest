using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace _01_Game.Scripts.Tools
{
    public class ClickHandler : MonoBehaviour, IPointerDownHandler
    {
        public UnityEvent onClick;
        public void OnPointerDown(PointerEventData eventData)
        {
            onClick?.Invoke();
        }
    }
}