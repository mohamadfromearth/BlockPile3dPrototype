using System.Collections.Generic;
using DG.Tweening;
using Event;
using Objects.Block;
using Scrips.Event;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Objects.BlocksContainer
{
    public class BlockContainer : MonoBehaviour, IBlockContainer, IPointerDownHandler, IPointerUpHandler
    {
        private Stack<IBlock> blocks = new();

        private int _countIndex = -1;

        private List<int> _countList = new List<int>();

        public Stack<Color> Colors { get; set; }

        public bool IsPlaced { get; set; }

        public void Destroy()
        {
            if (Colors.Count > 1)
            {
                var color = Colors.Peek();

                while (blocks.Count > 0)
                {
                    blocks.Pop().Destroy();

                    if (blocks.Peek().Color != color)
                    {
                        Colors.Pop();
                        break;
                    }
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }

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
            get
            {
                if (_countIndex >= 0)
                {
                    return _countList[_countIndex];
                }

                return 0;
            }
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

                _countList.Add(1);
                _countIndex++;

                return;
            }


            blockPosition = blocks.Peek().GetPosition();
            blockPosition.y += 0.2f;


            if (blocks.Peek().Color != block.Color)
            {
                Colors.Push(block.Color);

                _countList.Add(1);
                _countIndex++;
            }
            else
            {
                _countList[_countIndex] += 1;
            }

            blocks.Push(block);


            block.SetPosition(blockPosition);
            block.GameObj.transform.SetParent(transform);
        }

        public void Push(IBlock block, float duration)
        {
            Vector3 targetBlockPosition = GetPosition();
            var currentBlockPosition = block.GetPosition();
            Vector3[] points = new Vector3[3];
            Vector3 mid;


            targetBlockPosition.y += 0.2f * blocks.Count;

            if (blocks.Peek().Color != block.Color)
            {
                Colors.Push(block.Color);

                _countList.Add(1);
                _countIndex++;
            }
            else
            {
                if (blocks.Count == 0)
                {
                    _countList.Add(1);
                    _countIndex++;
                }
                else
                {
                    _countList[_countIndex] += 1;
                }
            }

            blocks.Push(block);

            mid = currentBlockPosition + (targetBlockPosition - currentBlockPosition) / 2f;
            mid.y = targetBlockPosition.y + 1f;


            points[0] = currentBlockPosition;
            points[1] = mid;
            points[2] = targetBlockPosition;

            var rotation = block.GameObj.transform.rotation.eulerAngles;
            rotation = rotation + GetTargetRotation(currentBlockPosition, targetBlockPosition);
            block.GameObj.transform.DOPath(points, duration, PathType.CatmullRom);
            block.GameObj.transform.DOLocalRotate(rotation, duration).onComplete += () =>
            {
                block.GameObj.transform.rotation = Quaternion.identity;
            };
            block.GameObj.transform.SetParent(transform);
        }

        private Vector3 GetTargetRotation(Vector3 currentPosition, Vector3 targetPosition)
        {
            var dif = targetPosition - currentPosition;


            if (Mathf.RoundToInt(dif.x) > 0)
            {
                return new Vector3(0f, 0f, -180f);
            }

            if (Mathf.RoundToInt(dif.x) < 0)
            {
                return new Vector3(0f, 0f, 180f);
            }

            if (dif.z > 0)
            {
                return new Vector3(180f, 0f, 0f);
            }

            if (dif.z < 0)
            {
                return new Vector3(-180f, 0f, 0f);
            }

            return Vector3.zero;
        }


        public IBlock Pop()
        {
            if (blocks.Count == 0) return null;

            var block = blocks.Pop();

            if (blocks.Count == 0)
            {
                Colors.Pop();

                _countList.RemoveAt(_countIndex);
                _countIndex = -1;

                return block;
            }

            if (blocks.Peek().Color != block.Color)
            {
                Colors.Pop();
                _countList.RemoveAt(_countList.Count - 1);
                _countIndex--;
            }
            else
            {
                _countList[_countIndex] -= 1;
            }

            return block;
        }


        public void OnPointerDown(PointerEventData eventData)
        {
            if (IsPlaced == true) return;
            Channel.Rise<CellContainerPointerDown>(new CellContainerPointerDown(this));
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (IsPlaced == true) return;
            Channel.Rise<CellContainerPointerUp>(new CellContainerPointerUp(this));
        }
    }
}