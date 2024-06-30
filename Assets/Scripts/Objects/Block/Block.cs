using UnityEngine;

namespace Objects.Block
{
    public class Block : MonoBehaviour, IBlock
    {
        [SerializeField] private MeshRenderer renderer;

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public Vector3 GetPosition() => transform.position;


        private Color color;

        public Color Color
        {
            set
            {
                foreach (var material in renderer.materials)
                {
                    material.color = value;
                }

                color = value;
            }
            get { return color; }
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        public GameObject GameObj
        {
            get => gameObject;
        }
    }
}