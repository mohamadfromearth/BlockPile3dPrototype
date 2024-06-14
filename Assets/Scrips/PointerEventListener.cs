using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Scrips
{
    public class PointerEventListener : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private UnityEvent<GameObject> pointerDownEvent;


        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("Pointer is down");
            pointerDownEvent?.Invoke(gameObject);
        }
    }
}