using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Objects.BlocksContainer;
using Scrips;
using Scrips.Objects.Cell;
using Scrips.Objects.CellsContainer;
using Scripts.Data;
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


    private const float BlockPlacementDuration = 0.60f;
    private const float BlockPlacementRate = 0.3f;
    private const float BlockPlacementDelay = 0.3f;


    private readonly WaitForSeconds _blockPlacementRateWaitForSeconds;
    private readonly WaitForSeconds _blockPlacementDelayWaitForSeconds;

    private const int MaxBlock = 7;

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


    public IEnumerator UpdateBoardRoutine(Vector3Int boardPosition)
    {
        var containers = GetMatchedContainers(boardPosition);
        if (containers.Count <= 1) yield break;

        var targetContainer = containers[^1].Value;

        var recentContainer = _board.GetBlockContainerHolder(boardPosition).BlockContainer;

        if (recentContainer.GameObj.GetInstanceID() != targetContainer.GameObj.GetInstanceID()
            && containers.Count != 2
           )
        {
            var firstList = new List<KeyValuePair<int, IBlockContainer>>(containers);

            firstList.RemoveAt(firstList.Count - 1);

            yield return ReArrangeRoutine(firstList, recentContainer);


            var secondList = new List<KeyValuePair<int, IBlockContainer>>()
            {
                new KeyValuePair<int, IBlockContainer>(0, recentContainer),
                containers[^1]
            };

            yield return ReArrangeRoutine(secondList, targetContainer);


            if (targetContainer.Count >= MaxBlock)
            {
                _board.AddBlockContainer(null, targetContainer.GetPosition());
                targetContainer.Destroy();
            }


            var adjacentMatchedContainers = GetMatchedContainers(_board.WorldToCell(containers[0].Value.GetPosition()));

            if (adjacentMatchedContainers.Count > 2)
            {
                RemoveContainer(adjacentMatchedContainers, recentContainer);

                yield return ReArrangeRoutine(adjacentMatchedContainers, adjacentMatchedContainers[^1].Value);
            }

            var singleRecentMatchedContainers = GetMatchedContainers(_board.WorldToCell(recentContainer.GetPosition()));

            if (singleRecentMatchedContainers.Count > 1)
            {
                yield return ReArrangeRoutine(singleRecentMatchedContainers, recentContainer);
            }

            if (recentContainer.Count >= MaxBlock)
            {
                recentContainer.Destroy();
                _board.AddBlockContainer(null, recentContainer.GetPosition());
            }

            List<KeyValuePair<int, IBlockContainer>> remainedSingleColors = new();

            foreach (var adjacentMatchedContainer in adjacentMatchedContainers)
            {
                if (adjacentMatchedContainer.Value.HasSingleColor)
                {
                    remainedSingleColors.Add(adjacentMatchedContainer);
                }
            }

            foreach (var remainedSingleColor in remainedSingleColors)
            {
                yield return UpdateBoardRoutine(_board.WorldToCell(remainedSingleColor.Value.GetPosition()));
            }
        }
        else
        {
            // REARRANGE

            yield return ReArrangeRoutine(containers, targetContainer);

            // REARRANGE


            var singleColorsContainers = new List<IBlockContainer>();

            foreach (var keyValuePair in containers)
            {
                if (keyValuePair.Value.GameObj.GetInstanceID() == targetContainer.GameObj.GetInstanceID())
                {
                    continue;
                }

                if (keyValuePair.Value.HasSingleColor)
                {
                    singleColorsContainers.Add(keyValuePair.Value);
                }
            }

            if (targetContainer.Count >= MaxBlock)
            {
                _board.AddBlockContainer(null, targetContainer.GetPosition());
                targetContainer.Destroy();
            }

            foreach (var singleColorsContainer in singleColorsContainers)
            {
                yield return UpdateBoardRoutine(_board.WorldToCell(singleColorsContainer.GetPosition()));
            }
        }
    }

    private static void RemoveContainer(List<KeyValuePair<int, IBlockContainer>> adjacentMatchedContainers,
        IBlockContainer recentContainer)
    {
        var removeIndex = 0;
        for (int i = 0; i < adjacentMatchedContainers.Count; i++)
        {
            var adjacentMatchedContainer = adjacentMatchedContainers[i];
            if (adjacentMatchedContainer.Value.GameObj.GetInstanceID() ==
                recentContainer.GameObj.GetInstanceID())
            {
                removeIndex = i;
                break;
            }
        }

        adjacentMatchedContainers.RemoveAt(removeIndex);
    }

    private IEnumerator ReArrangeRoutine(List<KeyValuePair<int, IBlockContainer>> containers,
        IBlockContainer targetContainer)
    {
        foreach (var containerValuePair in containers)
        {
            var container = containerValuePair.Value;

            if (container.GameObj.GetInstanceID() == targetContainer.GameObj.GetInstanceID()) continue;

            var block = container.Pop();

            var color = targetContainer.Colors.Peek();

            while (true)
            {
                targetContainer.Push(block, BlockPlacementDuration);

                yield return _blockPlacementRateWaitForSeconds;


                block = container.Peek();

                if (block == null)
                {
                    _board.AddBlockContainer(null, container.GetPosition());
                    break;
                }

                if (block.Color != color)
                {
                    break;
                }

                block = container.Pop();
            }

            yield return _blockPlacementDelayWaitForSeconds;
        }
    }


    private List<KeyValuePair<int, IBlockContainer>> GetMatchedContainers(Vector3Int boardPosition)
    {
        var recentPlacedContainer = _board.GetBlockContainerHolder(boardPosition).BlockContainer;

        List<KeyValuePair<int, IBlockContainer>> containers = new();

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