using Scrips.Objects.Cell;
using UnityEngine;

namespace Scrips.Objects.Block
{
    public class Block : MonoBehaviour, IBlock
    {
        [SerializeField] private MeshRenderer renderer;

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public Vector3 GetPosition() => transform.position;


        public Color Color
        {
            set
            {
                foreach (var material in renderer.materials)
                {
                    material.color = value;
                }
            }
            get { return Color; }
        }

        public GameObject GameObj
        {
            get => gameObject;
        }
    }
}