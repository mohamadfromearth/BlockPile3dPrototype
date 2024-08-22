using System.Collections;
using System.Collections.Generic;
using System.Linq;
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


        [SerializeField] private float height = 0.21f;
        [SerializeField] private float destroyRate = 0.08f;

        private WaitForSeconds _destroyRateWaitForSeconds;

        private Tween _pathTween;
        private Tween _rotateTween;


        private void Start()
        {
            _destroyRateWaitForSeconds = new WaitForSeconds(destroyRate);
        }

        public float Destroy(bool destroyImmediately)
        {
            if (destroyImmediately)
            {
                KillTweens();
                Object.Destroy(gameObject);
                _hasBeenDestroyed = true;
                return 0;
            }

            var blocksBuffers = new List<IBlock>();

            bool destroyContainer;

            if (Colors.Count > 1)
            {
                destroyContainer = false;
                var color = Colors.Peek();

                while (blocks.Count > 0)
                {
                    blocksBuffers.Add(blocks.Pop());

                    if (blocks.Peek().Color != color)
                    {
                        Colors.Pop();
                        break;
                    }
                }
            }
            else
            {
                destroyContainer = true;
                blocksBuffers = blocks.ToList();
                Colors.Clear();
                _hasBeenDestroyed = true;
            }

            StartCoroutine(DestroyAnimationRoutine(blocksBuffers, destroyContainer));

            return blocksBuffers.Count * destroyRate + 0.5f;
        }

        public void SetParent(Transform parent) => transform.SetParent(parent);

        private void KillTweens()
        {
            _rotateTween?.Kill();
            _pathTween?.Kill();
        }

        private IEnumerator DestroyAnimationRoutine(List<IBlock> blocksBuffer,
            bool destroyContainer)
        {
            int count = 0;

            foreach (var block in blocksBuffer)
            {
                block.Destroy();
                yield return _destroyRateWaitForSeconds;
                count++;
            }

            if (destroyContainer)
            {
                KillTweens();
                Object.Destroy(gameObject);
            }

            Channel.Rise<BlockDestroy>(new BlockDestroy(count));
        }

        private bool _hasBeenDestroyed = false;

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

        public bool WasUpperColorChanged { get; set; }

        private void Awake()
        {
            Colors = new();
            WasUpperColorChanged = false;
        }


        public EventChannel Channel { private get; set; }

        public GameObject GameObj
        {
            get
            {
                if (_hasBeenDestroyed) return null;
                return gameObject;
            }
        }


        public void SetPosition(Vector3 position) => transform.position = position;


        public Vector3 GetPosition()
        {
            if (_hasBeenDestroyed) return Vector3.zero;
            return transform.position;
        }

        public void Push(IBlock block)
        {
            Vector3 blockPosition;
            Vector3 midPoint;

            if (blocks.Count == 0)
            {
                blockPosition = GetPosition();
                blockPosition.y += height;

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
            blockPosition.y += height;


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
            if (_hasBeenDestroyed) return;

            Vector3 targetBlockPosition = GetPosition();
            var currentBlockPosition = block.GetPosition();
            Vector3[] points = new Vector3[3];
            Vector3 mid;


            targetBlockPosition.y += height * blocks.Count;

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

            if (targetBlockPosition.y < currentBlockPosition.y)
            {
                mid.y = currentBlockPosition.y + 1;
            }
            else
            {
                mid.y = targetBlockPosition.y + 1f;
            }


            points[0] = currentBlockPosition;
            points[1] = mid;
            points[2] = targetBlockPosition;

            var rotation = block.GameObj.transform.rotation.eulerAngles;
            rotation = rotation + GetTargetRotation(currentBlockPosition, targetBlockPosition);
            _pathTween = block.GameObj.transform.DOPath(points, duration, PathType.CatmullRom);
            _rotateTween = block.GameObj.transform.DOLocalRotate(rotation, duration);
            _rotateTween.onComplete += () => { block.GameObj.transform.rotation = Quaternion.identity; };
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

                WasUpperColorChanged = false;

                return block;
            }

            if (blocks.Peek().Color != block.Color)
            {
                Colors.Pop();
                _countList.RemoveAt(_countList.Count - 1);
                _countIndex--;
                WasUpperColorChanged = true;
            }
            else
            {
                _countList[_countIndex] -= 1;
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


        public Stack<IBlock> GetBlocks() => blocks;
    }
}