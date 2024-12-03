using Event;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Objects.AdvertiseBlock
{
    public class AdvertiseBlock : MonoBehaviour, IAdvertiseBlock, IPointerDownHandler
    {
        public EventChannel Channel { get; set; }


        public void Init()
        {
            Channel.Subscribe<GridRotate>(OnGridRotate);
        }


        private void OnDisable()
        {
            Channel.UnSubscribe<GridRotate>(OnGridRotate);
        }


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


        private void OnGridRotate()
        {
            //if (_hasBeenDestroyed) return;
            var rotation = Quaternion.Inverse(Channel.GetData<GridRotate>().Rotation);
            transform.rotation = Quaternion.Euler(90f, rotation.y + 45, 0);
        }
    }
}