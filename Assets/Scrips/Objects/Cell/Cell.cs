using UnityEngine;

namespace Scrips.Objects.Cell
{
    public class Cell : MonoBehaviour, ICell
    {
        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public Vector3 GetPosition() => transform.position;


        public Color Color { get; set; }

        public GameObject GameObj
        {
            get => gameObject;
        }
    }
}