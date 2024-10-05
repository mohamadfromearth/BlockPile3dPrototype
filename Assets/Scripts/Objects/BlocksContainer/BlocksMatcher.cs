using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using Event;
using Objects.Block;
using UnityEngine;
using Zenject;

namespace Objects.BlocksContainer
{
    public class BlocksMatcher : MonoBehaviour
    {
        [Inject] private IBlockContainerFactory _blockContainerFactory;
        [Inject] private IBlockFactory _blockFactory;
        [Inject] private ILevelRepository _levelRepository;
        [Inject] private Board _board;
        [Inject] private EventChannel _channel;


        private readonly Vector3Int[] _gridHorizontalVerticalOffsets = new[]
        {
            new Vector3Int(1, 0, 0),
            new Vector3Int(0, 0, -1),
            new Vector3Int(-1, 0, 0),
            new Vector3Int(0, 0, 1)
        };


        private const float BlockPlacementDuration = 0.35f;
        private const float BlockPlacementRate = 0.06f;
        private const float BlockPlacementDelay = 0.175f;


        private readonly WaitForSeconds _blockPlacementRateWaitForSeconds = new(BlockPlacementRate);
        private readonly WaitForSeconds _blockPlacementDelayWaitForSeconds = new(BlockPlacementDelay);

        private const int MaxBlock = 10;

        private bool _areBlocksMatching = false;


        private HashSet<Vector3Int> _checkedList = new();


        private List<Vector3Int> _blocksToMatch = new();

        public Vector3Int StartMatchingPosition { get; set; }

        public bool AreBlocksMatching() => _areBlocksMatching;


        public void Stop()
        {
            StopAllCoroutines();
        }

        private Queue<Vector3Int> _blocksToMatchQueue = new();


        private IEnumerator MatchBlock(Vector3Int boardPosition, Vector3Int exceptOffset)
        {
            var container = _board.GetCell(boardPosition).BlockContainer;

            _checkedList.Add(boardPosition);

            foreach (var gridOffset in _gridHorizontalVerticalOffsets)
            {
                var position = boardPosition + gridOffset;

                var holder = _board.GetCell(position);

                if (holder == null) continue;
                if (_checkedList.Contains(position)) continue;
                _checkedList.Add(position);

                var matchedContainer = holder.BlockContainer;

                if (gridOffset == exceptOffset) continue;

                if (matchedContainer != null)
                {
                    if (matchedContainer.Colors.Peek() == container.Colors.Peek())
                    {
                        yield return MatchBlock(position, gridOffset * -1);


                        var block = matchedContainer.Pop();
                        var color = container.Colors.Peek();

                        while (true)
                        {
                            container.Push(block, BlockPlacementDuration);
                            container.SetCountText("", 0);
                            matchedContainer.SetCountText("", 0);


                            block = matchedContainer.Peek();

                            if (block == null)
                            {
                                container.SetCountText(container.Count.ToString(), BlockPlacementDuration);
                                _board.AddBlockContainer(null, matchedContainer.GetPosition());
                                matchedContainer.SetCountText("", 0);
                                matchedContainer.Destroy();
                                yield return _blockPlacementDelayWaitForSeconds;

                                break;
                            }

                            if (block.Color != color)
                            {
                                container.SetCountText(container.Count.ToString(), BlockPlacementDuration);
                                matchedContainer.SetCountText(matchedContainer.Count.ToString(),
                                    0);
                                _checkedList.Add(_board.WorldToCell(matchedContainer.GetPosition()));
                                _blocksToMatch.Add(position);
                                yield return _blockPlacementDelayWaitForSeconds;

                                break;
                            }

                            block = matchedContainer.Pop();

                            yield return _blockPlacementRateWaitForSeconds;
                            yield return null;
                        }

                        yield return _blockPlacementDelayWaitForSeconds;
                    }
                }
            }
        }


        public IEnumerator UpdateBoardRoutine(Vector3Int boardPosition, bool isStaringPoint = false,
            bool isLastIndex = false, bool isFromQueue = false)
        {
            if (_areBlocksMatching && isFromQueue == false)
            {
                _blocksToMatchQueue.Enqueue(boardPosition);
                yield break;
            }

            _areBlocksMatching = true;

            var containers = GetMatchedContainers(boardPosition);

            if (containers.Count > 1)
            {
                var targetContainer = containers[^1].Value;

                yield return MatchBlock(_board.WorldToCell(targetContainer.GetPosition()), Vector3Int.zero);

                if (targetContainer.Count >= MaxBlock)
                {
                    var targetPosition = targetContainer.GetPosition();
                    var targetCount = targetContainer.Count;
                    targetContainer.SetCountText("", 0);
                    yield return new WaitForSeconds(targetContainer.Destroy());

                    _channel.Rise<TargetBlockDestroyed>(new TargetBlockDestroyed(targetPosition, targetCount));

                    if (targetContainer.Colors.Count == 0)
                    {
                        _board.AddBlockContainer(null, targetPosition);
                    }
                    else
                    {
                        var position = _board.WorldToCell(targetContainer.GetPosition());
                        _blocksToMatch.Add(position);
                    }
                }

                _checkedList.Clear();


                foreach (var keyValuePair in containers)
                {
                    if (keyValuePair.Value.WasUpperColorChanged)
                    {
                        keyValuePair.Value.WasUpperColorChanged = false;
                        if (keyValuePair.Value.GameObj == null) continue;
                        yield return UpdateBoardRoutine(_board.WorldToCell(keyValuePair.Value.GetPosition()));
                    }
                }

                if (isStaringPoint || isLastIndex || isFromQueue)
                {
                    for (int i = 0; i < _blocksToMatch.Count; i++)
                    {
                        var position = _blocksToMatch[i];
                        var cell = _board.GetCell(position);
                        if (cell.BlockContainer == null) continue;

                        cell.BlockContainer.WasUpperColorChanged = false;

                        isLastIndex = i == _blocksToMatch.Count - 1;


                        yield return UpdateBoardRoutine(position, false, isLastIndex);

                        if (isLastIndex)
                        {
                            _blocksToMatch.Clear();
                        }
                    }
                }

                if (isStaringPoint || isFromQueue)
                {
                    while (_blocksToMatchQueue.Count > 0)
                    {
                        yield return UpdateBoardRoutine(_blocksToMatchQueue.Dequeue(), false, false, true);
                    }
                }
            }

            if (isStaringPoint)
            {
                _areBlocksMatching = false;
                _channel.Rise<UpdateBoardCompleted>(new UpdateBoardCompleted());
            }
        }


        private List<KeyValuePair<int, IBlockContainer>> GetMatchedContainers(Vector3Int boardPosition)
        {
            var recentPlacedContainer = _board.GetCell(boardPosition)?.BlockContainer;

            List<KeyValuePair<int, IBlockContainer>> containers = new();


            if (recentPlacedContainer == null) return containers;

            int containerScore = 1;

            if (recentPlacedContainer.HasSingleColor)
            {
                containerScore += 10;
            }

            containers.Add(new KeyValuePair<int, IBlockContainer>(containerScore, recentPlacedContainer));


            foreach (var gridOffset in _gridHorizontalVerticalOffsets)
            {
                var neighbourPosition = boardPosition + gridOffset;
                var neighbour = _board.GetCell(neighbourPosition)?.BlockContainer;

                if (neighbour != null)
                {
                    if (neighbour.Colors.Peek() == recentPlacedContainer.Colors.Peek())
                    {
                        int neighbourScore = 0;

                        if (neighbour.HasSingleColor)
                        {
                            neighbourScore = 10;
                        }

                        containers.Add(new KeyValuePair<int, IBlockContainer>(neighbourScore, neighbour));
                    }
                }
            }

            containers = containers.OrderBy(pair => pair.Key).ToList();
            return containers;
        }
    }
}