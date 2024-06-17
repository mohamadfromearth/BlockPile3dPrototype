using System.Collections.Generic;
using Event;
using Scrips.Event;
using Scrips.Objects.Cell;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scrips.Objects.CellsContainer
{
    public class BlockContainer : MonoBehaviour, IBlockContainer, IPointerDownHandler, IPointerUpHandler
    {
        private Stack<IBlock> _cells = new Stack<IBlock>();

        public Color Color { get; set; }


        public EventChannel Channel { get; set; }

        public GameObject GameObj
        {
            get => gameObject;
        }


        public void SetPosition(Vector3 position) => transform.position = position;


        public Vector3 GetPosition() => transform.position;

        public void Push(IBlock block)
        {
            _cells.Push(block);
        }


        public IBlock Pop() => _cells.Pop();

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