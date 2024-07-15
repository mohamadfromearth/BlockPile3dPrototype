using System.Collections.Generic;
using System.Linq;
using Event;
using Objects.BlocksContainer;
using Scrips.Event;
using Scrips.Utils;
using Scripts.Data;
using UI;
using UnityEngine;
using Utils;
using Zenject;

public class GameManager : MonoBehaviour
{
    private Camera _camera;

    [Header("UI")] [SerializeField] private GameUI gameUI;
    [SerializeField] private WinUI winUI;

    [SerializeField] private LayerMask groundLayerMask;

    [SerializeField] private List<Transform> selectionBarCellContainerTransformList;
    private List<Vector3> _selectionBarCellContainerPosList = new();
    [SerializeField] private GameManagerHelpers helpers;
    [SerializeField] private BlocksMatcher blocksMatcher;

    [Inject] private Board _board;
    [Inject] private EventChannel _channel;
    [Inject] private ILevelRepository _levelRepository;
    [Inject] private BlockContainerSelectionBar _selectionBar;
    [Inject] private BlockContainersPlacer _blockContainersPlacer;


    private int _selectionBarSelectedIndex;
    private IBlockContainer _selectedBlockContainer;

    private float _currentScore;


    private void Awake()
    {
        _camera = Camera.main;

        Application.targetFrameRate = 60;
    }


    private void Start()
    {
        _selectionBarCellContainerPosList = selectionBarCellContainerTransformList.Select(t => t.position).ToList();
        _selectionBar.Spawn();

        _board.SpawnHolders(_levelRepository.GetLevelData().emptyHoldersPosList);

        _blockContainersPlacer.Place();

        gameUI.SetProgressText(helpers.GetTargetScoreString(_currentScore));
        gameUI.SetProgress(0);
    }

    private void OnEnable()
    {
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        UnSubscribeToEvents();
    }


    private void SubscribeToEvents()
    {
        _channel.Subscribe<CellContainerPointerDown>(OnCellContainerPointerDown);
        _channel.Subscribe<CellContainerPointerUp>(OnCellContainerPointerUp);
        _channel.Subscribe<BlockDestroy>(OnBlocksDestroyed);

        winUI.AddNextLevelClickListener(OnNextLevel);
    }

    private void UnSubscribeToEvents()
    {
        _channel.UnSubscribe<CellContainerPointerDown>(OnCellContainerPointerDown);
        _channel.UnSubscribe<CellContainerPointerUp>(OnCellContainerPointerUp);
        _channel.UnSubscribe<BlockDestroy>(OnBlocksDestroyed);

        winUI.RemoveNextLevelClickListener(OnNextLevel);
    }


    #region Subscribers

    private void OnCellContainerPointerDown()
    {
        _selectedBlockContainer = _channel.GetData<CellContainerPointerDown>().BlockContainer;

        _selectionBarSelectedIndex =
            ListUtils.GetIndex(_selectedBlockContainer.GetPosition(), _selectionBarCellContainerPosList);
    }

    private void OnCellContainerPointerUp()
    {
        if (_selectedBlockContainer != null)
        {
            var pos = _selectedBlockContainer.GetPosition();
            pos.x += 0.4f;
            pos.z += 0.4f;

            var holder = _board.GetBlockContainerHolder(pos);

            if (holder != null && holder.BlockContainer == null)
            {
                _selectedBlockContainer.IsPlaced = true;
                _selectedBlockContainer.SetPosition(holder.GetPosition());

                _board.AddBlockContainer(_selectedBlockContainer, pos);

                var boardPosition = _board.WorldToCell(holder.GetPosition());
                blocksMatcher.StartMatchingPosition = boardPosition;
                StartCoroutine(blocksMatcher.UpdateBoardRoutine(boardPosition, true));

                _selectedBlockContainer = null;

                _selectionBar.Decrease();

                if (_selectionBar.Count == 0) _selectionBar.Spawn();
            }
            else
            {
                _selectedBlockContainer.SetPosition(_selectionBarCellContainerPosList[_selectionBarSelectedIndex]);
                _selectedBlockContainer = null;
            }
        }
    }

    private void OnBlocksDestroyed()
    {
        var data = _channel.GetData<BlockDestroy>();

        _currentScore += data.Count;

        gameUI.SetProgress(_currentScore / _levelRepository.GetLevelData().targetScore);
        gameUI.SetProgressText(helpers.GetTargetScoreString(_currentScore));

        CheckWin();
    }

    private void OnNextLevel()
    {
        _board.Clear();
        _levelRepository.NextLevel();
        _blockContainersPlacer.Place();
        winUI.Hide();
        gameUI.SetProgressText(helpers.GetTargetScoreString(_currentScore));
        gameUI.SetProgress(0);
    }

    public void OnPointerMove(Vector2 position)
    {
        if (_selectedBlockContainer != null)
        {
            var cellPos = PositionConverters.ScreenToWorldPosition(position, _camera, groundLayerMask);
            if (cellPos != null)
            {
                _selectedBlockContainer.SetPosition(cellPos.Value);
            }
        }
    }

    #endregion


    private void CheckWin()
    {
        if (_currentScore >= _levelRepository.GetLevelData().targetScore)
        {
            _currentScore = 0;
            winUI.Show();
        }
    }
}