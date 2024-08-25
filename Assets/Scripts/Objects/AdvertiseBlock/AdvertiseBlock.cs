using Event;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Objects.AdvertiseBlock
{
    public class AdvertiseBlock : MonoBehaviour, IAdvertiseBlock, IPointerDownHandler
    {
        public EventChannel Channel { get; set; }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public void Destroy() => Destroy(gameObject);

        public void OnPointerDown(PointerEventData eventData)
        {
            Channel.Rise<AdvertiseBlockPointerDown>(new AdvertiseBlockPointerDown(this));
        }

        public GameObject GameObj => gameObject;
    }
}