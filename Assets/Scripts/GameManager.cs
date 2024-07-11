using System.Collections.Generic;
using System.Linq;
using Event;
using Objects.BlocksContainer;
using Scrips;
using Scrips.Event;
using Scrips.Utils;
using Scripts.Data;
using UnityEngine;
using Utils;
using Zenject;

public class GameManager : MonoBehaviour
{
    private Camera _camera;

    [SerializeField] private LayerMask groundLayerMask;

    [SerializeField] private List<Transform> selectionBarCellContainerTransformList;
    private List<Vector3> _selectionBarCellContainerPosList = new();
    [SerializeField] private GameManagerHelpers helpers;

    [Inject] private Board _board;
    [Inject] private EventChannel _channel;
    [Inject] private ILevelRepository _levelRepository;
    [Inject] private BlockContainerSelectionBar _selectionBar;


    private int _selectionBarSelectedIndex;
    private IBlockContainer _selectedBlockContainer;


    private void Awake()
    {
        _camera = Camera.main;

        Application.targetFrameRate = 60;
    }


    private void Start()
    {
        _selectionBarCellContainerPosList = selectionBarCellContainerTransformList.Select(t => t.position).ToList();
        //helpers.SpawnSelectionBarBlockContainers(_selectionBarCellContainerPosList);
        _selectionBar.Spawn();
        helpers.SpawnBoardBlockContainers();
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
    }

    private void UnSubscribeToEvents()
    {
        _channel.UnSubscribe<CellContainerPointerDown>(OnCellContainerPointerDown);
        _channel.UnSubscribe<CellContainerPointerUp>(OnCellContainerPointerUp);
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
                helpers.StartMatchingPosition = boardPosition;
                StartCoroutine(helpers.UpdateBoardRoutine(boardPosition, true));

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
}