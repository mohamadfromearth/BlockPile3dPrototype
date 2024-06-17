using Scrips.Objects.CellsContainer;
using UnityEngine;

namespace Scrips.Objects.BlockContainerHolder
{
    public class BlockContainerHolder : MonoBehaviour, IBlockContainerHolder
    {
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
    }
}