using System;
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


    private float _blockPlacementDuration = 0.5f;

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


    public void UpdateBoard(Vector3Int boardPosition)
    {
        var containers = GetMatchedContainers(boardPosition);
        if (containers.Count <= 1) return;

        StartCoroutine(ReArrangeBoard(containers, (() =>
        {
            var singleColorsContainers = new List<IBlockContainer>();

            foreach (var keyValuePair in containers)
            {
                if (keyValuePair.Value.HasSingleColor)
                {
                    singleColorsContainers.Add(keyValuePair.Value);
                }
            }

            foreach (var singleColorsContainer in singleColorsContainers)
            {
                UpdateBoard(_board.WorldToCell(singleColorsContainer.GetPosition()));
            }
        })));
    }


    public IEnumerator UpdateBoardRoutine(Vector3Int boardPosition)
    {
        var containers = GetMatchedContainers(boardPosition);
        if (containers.Count <= 1) yield break;

        var targetContainer = containers[^1].Value;

        var recentContainer = _board.GetBlockContainerHolder(boardPosition).BlockContainer;

        if (recentContainer.GameObj.GetInstanceID() != targetContainer.GameObj.GetInstanceID())
        {
            var adjacentContainer =
                recentContainer.GameObj.GetInstanceID() == containers[0].Value.GameObj.GetInstanceID()
                    ? containers[1]
                    : containers[0];
            
            
            
        }
        else
        {
            // REARRANGE


            foreach (var containerValuePair in containers)
            {
                var container = containerValuePair.Value;

                if (container.GameObj.GetInstanceID() == targetContainer.GameObj.GetInstanceID()) continue;

                var block = container.Pop();

                var color = targetContainer.Colors.Peek();

                while (true)
                {
                    targetContainer.Push(block, _blockPlacementDuration);

                    yield return new WaitForSeconds(_blockPlacementDuration);


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
            }

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

            _board.AddBlockContainer(null, targetContainer.GetPosition());
            targetContainer.Destroy();

            foreach (var singleColorsContainer in singleColorsContainers)
            {
                yield return UpdateBoardRoutine(_board.WorldToCell(singleColorsContainer.GetPosition()));
            }
        }
    }

    private IEnumerator ReArrangeBoard(List<KeyValuePair<int, IBlockContainer>> containers, Action onComplete)
    {
        // a+(b-a)/2


        var targetContainer = containers[^1].Value;

        foreach (var containerValuePair in containers)
        {
            var container = containerValuePair.Value;

            if (container.GameObj.GetInstanceID() == targetContainer.GameObj.GetInstanceID()) continue;

            var block = container.Pop();

            var color = targetContainer.Colors.Peek();

            while (true)
            {
                targetContainer.Push(block, _blockPlacementDuration);

                yield return new WaitForSeconds(_blockPlacementDuration);


                block = container.Peek();

                if (block == null)
                {
                    _board.AddBlockContainer(null, container.GetPosition());
                    onComplete.Invoke();
                    yield break;
                }

                if (block.Color != color)
                {
                    onComplete.Invoke();
                    yield break;
                }

                block = container.Pop();
            }
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