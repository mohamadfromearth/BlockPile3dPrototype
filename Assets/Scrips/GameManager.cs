using System.Collections.Generic;
using System.Linq;
using Event;
using Scrips.Data;
using Scrips.Event;
using Scrips.Objects.CellsContainer;
using Scrips.Utils;
using UnityEngine;
using Zenject;

namespace Scrips
{
    public class GameManager : MonoBehaviour
    {
        private Camera _camera;

        [SerializeField] private LayerMask groundLayerMask;

        [SerializeField] private List<Transform> selectionBarCellContainerTransformList;
        private List<Vector3> _selectionBarCellContainerPosList = new();


        [Inject] private GameManagerHelpers _helpers;
        [Inject] private Board _board;
        [Inject] private EventChannel _channel;
        [Inject] private ILevelRepository _levelRepository;


        private int _selectionBarSelectedIndex;
        private IBlockContainer _selectedBlockContainer;


        private void Awake()
        {
            _camera = Camera.main;
        }


        private void Start()
        {
            _selectionBarCellContainerPosList = selectionBarCellContainerTransformList.Select(t => t.position).ToList();
            _helpers.SpawnSelectionBarBlockContainers(_selectionBarCellContainerPosList);
            _helpers.SpawnBoardBlockContainers();
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
                    _selectedBlockContainer.SetPosition(holder.GetPosition());

                    _board.AddBlockContainer(_selectedBlockContainer, pos);


                    _selectedBlockContainer = null;
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

                    var holder = _board.GetBlockContainerHolder(cellPos.Value);

                    if (holder != null)
                    {
                    }
                }
            }
        }

        #endregion
    }
}