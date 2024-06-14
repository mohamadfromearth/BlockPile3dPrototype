using Scrips.Objects.CellsContainer;
using UnityEngine;

namespace Scrips.Objects.CellContainerHolder
{
    public class CellContainerHolder : MonoBehaviour, ICellContainerHolder
    {
        public ICellContainer CellContainer { get; set; }

        public void SetPosition(Vector3 position) => transform.position = position;

        public Vector3 GetPosition() => transform.position;
    }
}