using UnityEngine;

namespace Objects.NoneValueLockBlock
{
    public class NoneValueLockBlock : MonoBehaviour, INoneValueLockBlock
    {
        public void Destroy() => Destroy(gameObject);
        public void SetPosition(Vector3 position) => transform.position = position;

        public Vector3 GetPosition() => transform.position;
    }
}