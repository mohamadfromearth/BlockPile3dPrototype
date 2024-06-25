using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Event;
using Scrips.Event;
using Scrips.Objects.Block;
using Scrips.Objects.BlocksContainer;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils;

namespace Objects.BlocksContainer
{
    public class BlockContainer : MonoBehaviour, IBlockContainer, IPointerDownHandler, IPointerUpHandler
    {
        private Stack<IBlock> blocks = new();

        public Stack<Color> Colors { get; set; }

        public IBlock Peek()
        {
            if (blocks.Count == 0) return null;
            return blocks.Peek();
        }

        public bool HasSingleColor
        {
            get => Colors.Count == 1;
        }

        public int Count
        {
            get => Colors.Count;
        }

        private void Awake()
        {
            Colors = new();
        }


        public EventChannel Channel { get; set; }

        public GameObject GameObj
        {
            get => gameObject;
        }


        public void SetPosition(Vector3 position) => transform.position = position;


        public Vector3 GetPosition() => transform.position;

        public void Push(IBlock block)
        {
            Vector3 blockPosition;
            Vector3 midPoint;

            if (blocks.Count == 0)
            {
                blockPosition = GetPosition();
                blockPosition.y += 0.2f;

                midPoint = block.GetPosition() + (blockPosition - block.GetPosition()) / 2;
                block.SetPosition(blockPosition);
                Colors.Push(block.Color);
                block.GameObj.transform.SetParent(transform);
                blocks.Push(block);

                return;
            }


            blockPosition = blocks.Peek().GetPosition();
            blockPosition.y += 0.2f;


            if (blocks.Peek().Color != block.Color)
            {
                Colors.Push(block.Color);
            }

            blocks.Push(block);


            block.SetPosition(blockPosition);
            block.GameObj.transform.SetParent(transform);
        }

        public void Push(IBlock block, float duration)
        {
            Vector3 blockPosition;
            // a+(b-a)/2
            if (blocks.Count == 0)
            {
                blockPosition = GetPosition();
                blockPosition.y += 0.2f;

                block.GameObj.transform.DOMove(blockPosition, duration);

                Colors.Push(block.Color);
                block.GameObj.transform.SetParent(transform);
                blocks.Push(block);
                return;
            }


            blockPosition = blocks.Peek().GetPosition();
            blockPosition.y += 0.2f;


            if (blocks.Peek().Color != block.Color)
            {
                Colors.Push(block.Color);
            }

            blocks.Push(block);


            block.GameObj.transform.DOMove(blockPosition, duration);
            block.GameObj.transform.SetParent(transform);
        }

       
        public IBlock Pop()
        {
            if (blocks.Count == 0) return null;

            var block = blocks.Pop();

            if (blocks.Count == 0)
            {
                Debug.Log("Color is Over " + Colors.Count + Colors.Peek());
                Colors.Pop();
                return block;
            }

            if (blocks.Peek().Color != block.Color)
            {
                Colors.Pop();
                Debug.Log("Color is changed " + Colors.Count);
            }

            return block;
        }


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