using System.Collections.Generic;
using Event;
using Scrips.Event;
using Scrips.Objects.Cell;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scrips.Objects.CellsContainer
{
    public class CellContainer : MonoBehaviour, ICellContainer, IPointerDownHandler, IPointerUpHandler
    {
        private Stack<ICell> _cells = new Stack<ICell>();

        public Color Color { get; set; }


        public EventChannel Channel { get; set; }

        public GameObject GameObj
        {
            get => gameObject;
        }


        public void SetPosition(Vector3 position) => transform.position = position;


        public Vector3 GetPosition() => transform.position;

        public void Push(ICell cell)
        {
            _cells.Push(cell);
        }


        public ICell Pop() => _cells.Pop();

        public void OnPointerDown(PointerEventData eventData)
        {
            Channel.Rise<CellContainerPointerDown>(new CellContainerPointerDown(this));
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Channel.Rise<CellContainerPointerUp>(new CellContainerPointerUp(this));
        }
    }
}