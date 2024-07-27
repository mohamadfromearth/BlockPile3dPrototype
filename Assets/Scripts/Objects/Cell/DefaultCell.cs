using Event;
using Objects.BlocksContainer;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Objects.Cell
{
    public class DefaultCell : MonoBehaviour, ICell, IPointerClickHandler
    {
        [SerializeField] private new MeshRenderer renderer;
        [SerializeField] private Color color;

        public bool canPlaceItem = true;

        public bool CanPlaceItem
        {
            get => canPlaceItem;
            set => canPlaceItem = value;
        }

        public EventChannel Channel { private get; set; }

        private void Start()
        {
            UpdateColor();
        }

        private void UpdateColor()
        {
            foreach (var rendererMaterial in renderer.materials)
            {
                rendererMaterial.color = color;
            }
        }

        public IBlockContainer BlockContainer { get; set; }

        public void SetColor(Color color)
        {
            this.color = color;
            UpdateColor();
        }

        public Color GetColor() => color;


        public void Destroy() => Destroy(gameObject);

        public void SetPosition(Vector3 position) => transform.position = position;

        public Vector3 GetPosition() => transform.position;

        public GameObject GameObj
        {
            get => gameObject;
        }

        public void ToScale(float duration, Vector3 scale)
        {
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Channel.Rise<CellClick>(new CellClick(this));
        }
    }
}