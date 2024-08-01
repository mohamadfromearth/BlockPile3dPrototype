using UnityEngine;

namespace Objects.AdvertiseBlock
{
    public class AdvertiseBlock : MonoBehaviour, IAdvertiseBlock
    {
        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public void Destroy() => Destroy(gameObject);
    }
}