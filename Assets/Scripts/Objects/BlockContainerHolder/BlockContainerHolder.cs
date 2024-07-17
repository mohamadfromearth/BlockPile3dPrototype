using Event;
using Objects.BlocksContainer;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Objects.BlockContainerHolder
{
    public class BlockContainerHolder : MonoBehaviour, IBlockContainerHolder, IPointerClickHandler
    {
        [SerializeField] private new MeshRenderer renderer;
        [SerializeField] private Color color;

        public EventChannel Channel { private get; set; }

        private void Start()
        {
            foreach (var rendererMaterial in renderer.materials)
            {
                rendererMaterial.color = color;
            }
        }

        public IBlockContainer BlockContainer { get; set; }

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
        }
    }
}