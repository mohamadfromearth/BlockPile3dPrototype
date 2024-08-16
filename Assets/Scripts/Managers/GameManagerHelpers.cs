using System.Collections.Generic;
using Cinemachine;
using Data;
using Objects.BlocksContainer;
using UnityEngine;
using Zenject;

public class GameManagerHelpers : MonoBehaviour
{
    [Inject] private ILevelRepository _levelRepository;
    [Inject] private Board _board;

    private Vector3Int[] _horizontalGridOffsets = new[]
    {
        new Vector3Int(1, 0, 0),
        new Vector3Int(-1, 0, 0)
    };

    private Vector3Int[] _verticalGridOffsets = new[]
    {
        new Vector3Int(0, 0, 1),
        new Vector3Int(0, 0, -1)
    };

    public string GetTargetScoreString(float currentScore)
    {
        var levelData = _levelRepository.GetLevelData();

        return currentScore + "/" + levelData.targetScore;
    }


    // Whenever we go to get anotherChance state we need to destroy 3 blocks this method returns 
    // 3 adjacent blocks with a given coordinate;
    public List<IBlockContainer> GetAnotherChanceBlocks(Board board, Vector3Int startingPoint)
    {
        var isHorizontal = Random.value > 0.5;

        var horizontalList = new List<IBlockContainer>();
        var verticalList = new List<IBlockContainer>();
        Vector3Int pos;

        foreach (var horizontalGridOffset in _horizontalGridOffsets)
        {
            pos = startingPoint + horizontalGridOffset;

            var cell = board.GetCell(pos);

            if (cell != null && cell.BlockContainer != null)
            {
                horizontalList.Add(cell.BlockContainer);
            }
        }

        foreach (var verticalGridOffset in _verticalGridOffsets)
        {
            pos = startingPoint + verticalGridOffset;

            var cell = board.GetCell(pos);

            if (cell != null && cell.BlockContainer != null)
            {
                verticalList.Add(cell.BlockContainer);
            }
        }

        var blockContainers =
            isHorizontal && horizontalList.Count >= verticalList.Count ? horizontalList : verticalList;

        return blockContainers;
    }
    
}