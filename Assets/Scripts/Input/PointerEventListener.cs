using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Input
{
    public class PointerEventListener : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private UnityEvent<GameObject> pointerDownEvent;


        public void OnPointerDown(PointerEventData eventData)
        {
            pointerDownEvent?.Invoke(gameObject);
        }
    }
}