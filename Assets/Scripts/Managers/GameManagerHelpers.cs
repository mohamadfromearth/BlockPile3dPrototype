using System.Collections;
using System.Collections.Generic;
using Data;
using Objects.BlocksContainer;
using UI;
using UnityEngine;
using Zenject;

public class GameManagerHelpers : MonoBehaviour
{
    [Inject] private ILevelRepository _levelRepository;
    [Inject] private MainRepository _mainRepository;
    [Inject] private AbilityRepository _abilityRepository;
    [Inject] private CurrencyRepository _currencyRepository;
    [Inject] private Board _board;
    [SerializeField] private BlocksMatcher blockMatcher;

    [SerializeField] private Transform gridPivot;

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

    public void UpdateAbilityButtons(GameUI gameUI)
    {
        gameUI.SetPunchIntractable(_levelRepository.LevelIndex >=
                                   _abilityRepository.GetAbilityData(AbilityType.Punch).unLockLevel);
        gameUI.SetSwapIntractable(_levelRepository.LevelIndex >=
                                  _abilityRepository.GetAbilityData(AbilityType.Swap).unLockLevel);
        gameUI.SetRefreshIntractable(_levelRepository.LevelIndex >=
                                     _abilityRepository.GetAbilityData(AbilityType.Refresh).unLockLevel);

        gameUI.SetCoinText("Coin:" + _currencyRepository.GetCoin());
        gameUI.SetPunchButtonText("Punch" + _abilityRepository.GetAbilityData(AbilityType.Punch).count);
        gameUI.SetSwapButtonText("Swap" + _abilityRepository.GetAbilityData(AbilityType.Swap).count);
        gameUI.SetRefreshButtonText("Refresh" + _abilityRepository.GetAbilityData(AbilityType.Refresh).count);
    }

    public Vector3 ModifyBlockContainerPositionForRotatedGrid(Vector3 position)
    {
        if (gridPivot.rotation.eulerAngles.y == 0)
        {
            return position;
        }

        if (gridPivot.rotation.eulerAngles.y == 90)
        {
            position.z -= 1;
            return position;
        }

        if (gridPivot.rotation.eulerAngles.y == 180)
        {
            position.x -= 1;
            position.z -= 1;
            return position;
        }

        if (gridPivot.rotation.eulerAngles.y == 270)
        {
            position.x -= 1;
            return position;
        }

        return position;
    }


    public IEnumerator UpdateAllBoard()
    {
        var cells = _board.Cells;
        foreach (var keyValuePair in cells)
        {
            var cell = keyValuePair.Value;
            if (cell.BlockContainer != null)
            {
                yield return blockMatcher.UpdateBoardRoutine(_board.WorldToCell(cell.GetPosition()), true);
            }
        }
    }
}