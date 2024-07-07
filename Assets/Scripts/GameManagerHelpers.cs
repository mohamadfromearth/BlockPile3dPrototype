using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Objects.BlocksContainer;
using Scrips;
using Scrips.Objects.Cell;
using Scrips.Objects.CellsContainer;
using Scripts.Data;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class GameManagerHelpers : MonoBehaviour
{
    [Inject] private IBlockContainerFactory _blockContainerFactory;
    [Inject] private IBlockFactory _blockFactory;
    [Inject] private ILevelRepository _levelRepository;
    [Inject] private Board _board;


    private readonly Vector3Int[] _gridHorizontalVerticalOffsets = new[]
    {
        new Vector3Int(1, 0, 0),
        new Vector3Int(0, 0, -1),
        new Vector3Int(-1, 0, 0),
        new Vector3Int(0, 0, 1)
    };


    private const float BlockPlacementDuration = 0.35f;
    private const float BlockPlacementRate = 0.175f;
    private const float BlockPlacementDelay = 0.175f;


    private readonly WaitForSeconds _blockPlacementRateWaitForSeconds;
    private readonly WaitForSeconds _blockPlacementDelayWaitForSeconds;

    private const int MaxBlock = 7;


    private HashSet<Vector3Int> _checkedList = new();

    public GameManagerHelpers()
    {
        _blockPlacementRateWaitForSeconds = new WaitForSeconds(BlockPlacementRate);
        _blockPlacementDelayWaitForSeconds = new WaitForSeconds(BlockPlacementDelay);
    }

    public void SpawnSelectionBarBlockContainers(List<Vector3> blockContainersPositionList)
    {
        var containerDataList = _levelRepository.GetLevelData().selectionBarBlockContainerDataList;

        for (int i = 0; i < containerDataList.Count; i++)
        {
            var containerData = containerDataList[i];
            var container = _blockContainerFactory.Create();
            container.SetPosition(blockContainersPositionList[i]);

            foreach (var color in containerData.color)
            {
                var block = _blockFactory.Create();
                block.Color = color;
                container.Push(block);
            }
        }
    }

    public void SpawnBoardBlockContainers()
    {
        var levelData = _levelRepository.GetLevelData();
        var containerDataList = levelData.blockContainerDataList;

        foreach (var containerData in containerDataList)
        {
            var holder = _board.GetBlockContainerHolder(containerData.position);

            var container = _blockContainerFactory.Create();
            container.IsPlaced = true;

            holder.BlockContainer = container;

            container.SetPosition(holder.GetPosition());

            foreach (var color in containerData.color)
            {
                var block = _blockFactory.Create();
                block.Color = color;

                container.Push(block);
            }
        }
    }


    private IEnumerator MatchBlock(Vector3Int boardPosition, Vector3Int exceptOffset)
    {
        var container = _board.GetBlockContainerHolder(boardPosition).BlockContainer;

        foreach (var gridOffset in _gridHorizontalVerticalOffsets)
        {
            var position = boardPosition + gridOffset;

            var holder = _board.GetBlockContainerHolder(position);

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

                        yield return _blockPlacementRateWaitForSeconds;


                        block = matchedContainer.Peek();

                        if (block == null)
                        {
                            _board.AddBlockContainer(null, matchedContainer.GetPosition());
                            break;
                        }

                        if (block.Color != color)
                        {
                            break;
                        }

                        block = matchedContainer.Pop();

                        yield return null;
                    }

                    yield return _blockPlacementDelayWaitForSeconds;
                }
            }
        }
    }


    public IEnumerator UpdateBoardRoutine(Vector3Int boardPosition)
    {
        var containers = GetMatchedContainers(boardPosition);
        if (containers.Count > 1)
        {
            var targetContainer = containers[^1].Value;

            yield return MatchBlock(_board.WorldToCell(targetContainer.GetPosition()), Vector3Int.zero);


            if (targetContainer.Count >= MaxBlock)
            {
                var targetPosition = targetContainer.GetPosition();
                targetContainer.Destroy();

                if (targetContainer.Colors.Count == 0)
                {
                    _board.GetBlockContainerHolder(targetPosition).BlockContainer = null;
                    containers.RemoveAt(containers.Count - 1);
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
        }
    }


    private List<KeyValuePair<int, IBlockContainer>> GetMatchedContainers(Vector3Int boardPosition)
    {
        var recentPlacedContainer = _board.GetBlockContainerHolder(boardPosition).BlockContainer;

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
            var neighbour = _board.GetBlockContainerHolder(neighbourPosition)?.BlockContainer;

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