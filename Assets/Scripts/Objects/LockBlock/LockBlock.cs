using TMPro;
using UnityEngine;

namespace Objects.LockBlock
{
    public class LockBlock : MonoBehaviour, ILockBlock
    {
        [SerializeField] private TextMeshPro text;

        public void SetPosition(Vector3 position) => transform.position = position;


        public Vector3 GetPosition() => transform.position;

        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                text.text = _count.ToString();
            }
        }

        public void Destroy() => Destroy(gameObject);


        private int _count;
    }
}