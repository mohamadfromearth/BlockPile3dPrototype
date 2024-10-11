using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using DG.Tweening;
using Objects.BlocksContainer;
using UI;
using UnityEngine;
using Utils;
using Zenject;
using Random = UnityEngine.Random;

public class GameManagerHelpers : MonoBehaviour
{
    [Inject] private ILevelRepository _levelRepository;
    [Inject] private MainRepository _mainRepository;
    [Inject] private AbilityRepository _abilityRepository;
    [Inject] private CurrencyRepository _currencyRepository;
    [Inject] private Board _board;
    [SerializeField] private BlocksMatcher blockMatcher;
    [SerializeField] private Transform blockToProgressImage;
    [SerializeField] private Transform progressImageTransform;
    [SerializeField] private GameObject hammer;
    [SerializeField] private Camera camera;
    [SerializeField] private Vector3 abilityModeCameraRotation;

    [SerializeField] private float cameraRotationDuration = 0.6f;

    [SerializeField] private LayerMask layer;
    [SerializeField] private Transform gridPivot;

    [SerializeField] private Vector3 hammerInitialRotation;
    [SerializeField] private Vector3 hammerHitRotation;

    private Vector3 _progressImagePosition;

    private Vector3 _cameraDefaultRotation;
    private bool _isShuffling = false;

    public bool IsShuffling => _isShuffling;

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


    private void Start()
    {
        _progressImagePosition =
            PositionConverters.ScreenToWorldPosition(progressImageTransform.position, camera, layer).Value;

        _cameraDefaultRotation = camera.transform.rotation.eulerAngles;
    }

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
        var level = _levelRepository.LevelIndex + 1;
        gameUI.SetPunchIntractable(level >=
                                   _abilityRepository.GetAbilityData(AbilityType.Punch).unLockLevel);
        gameUI.SetSwapIntractable(level >=
                                  _abilityRepository.GetAbilityData(AbilityType.Swap).unLockLevel);
        gameUI.SetRefreshIntractable(level >=
                                     _abilityRepository.GetAbilityData(AbilityType.Refresh).unLockLevel);

        gameUI.SetCoinText(_currencyRepository.GetCoin().ToString());
        gameUI.SetPunchCountText(_abilityRepository.GetAbilityData(AbilityType.Punch).count.ToString());
        gameUI.SetSwapCountText(_abilityRepository.GetAbilityData(AbilityType.Swap).count.ToString());
        gameUI.SetRefreshCountText(_abilityRepository.GetAbilityData(AbilityType.Refresh).count.ToString());

        gameUI.SetPunchUnLockLevelText("Level " + _abilityRepository.GetAbilityData(AbilityType.Punch).unLockLevel);
        gameUI.SetSwapUnLockLevelText("Level " + _abilityRepository.GetAbilityData(AbilityType.Swap).unLockLevel);
        gameUI.SetRefreshUnLockLevelText("Level " + _abilityRepository.GetAbilityData(AbilityType.Refresh).unLockLevel);
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


    public void ShuffleBoard()
    {
        _isShuffling = true;
        StartCoroutine(ShuffleBoardRoutine());
    }

    public void ChangeCameraToAbilitiesState()
    {
        camera.transform.DORotate(abilityModeCameraRotation, cameraRotationDuration);
    }

    public void ChangeCameraToDefaultState()
    {
        camera.transform.DORotate(_cameraDefaultRotation, cameraRotationDuration);
    }


    public void ShowHammerAnimation(IBlockContainer containerBlock, TweenCallback action)
    {
        hammer.SetActive(true);
        var hammerPos = containerBlock.GetPosition();
        hammerPos.y = containerBlock.Peek().GetPosition().y + 1.3f;
        hammer.transform.position = hammerPos;
        hammer.transform.rotation = Quaternion.Euler(hammerInitialRotation);
        hammer.transform.DORotate(hammerHitRotation, 0.6f).SetEase(Ease.InBack).onComplete = action;
    }

    private IEnumerator ShuffleBoardRoutine()
    {
        yield return new WaitForSeconds(_board.Shuffle());

        var cells = _board.Cells;
        foreach (var keyValuePair in cells)
        {
            var cell = keyValuePair.Value;
            if (cell.BlockContainer != null)
            {
                yield return blockMatcher.UpdateBoardRoutine(_board.WorldToCell(cell.GetPosition()), true);
            }
        }

        _isShuffling = false;
    }
}