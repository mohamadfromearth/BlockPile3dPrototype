using System.Collections.Generic;
using System.Linq;
using Event;
using Scrips;
using Scrips.Event;
using Scrips.Objects.BlocksContainer;
using Scrips.Utils;
using Scripts.Data;
using UnityEngine;
using Zenject;

namespace Scripts
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

        private readonly Vector3Int[] _gridHorizontalVerticalOffsets = new[]
        {
            new Vector3Int(1, 0, 0),
            new Vector3Int(0, 0, -1),
            new Vector3Int(-1, 0, 0),
            new Vector3Int(0, 0, 1)
        };

        private readonly Vector3Int[] _griDiagonalOffsets = new[]
        {
            new Vector3Int(1, 0, 1),
            new Vector3Int(1, 0, -1),
            new Vector3Int(-1, 0, -1),
            new Vector3Int(-1, 0, 1)
        };


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

                    Check(_board.WorldToCell(holder.GetPosition()));


                    _selectedBlockContainer = null;
                }
                else
                {
                    _selectedBlockContainer.SetPosition(_selectionBarCellContainerPosList[_selectionBarSelectedIndex]);
                    _selectedBlockContainer = null;
                }
            }
        }


        private void Check(Vector3Int boardPosition)
        {
            var container = _board.GetBlockContainerHolder(boardPosition).BlockContainer;


            List<IBlockContainer> matchedContainers = new();

            foreach (var gridOffset in _gridHorizontalVerticalOffsets)
            {
                var neighbourPosition = boardPosition + gridOffset;
                var neighbour = _board.GetBlockContainerHolder(neighbourPosition)?.BlockContainer;


                if (neighbour != null)
                {
                    Debug.Log("neighbour colors count: " + neighbour.Colors.Count);
                    if (neighbour.Colors.Peek() == container.Colors.Peek())
                    {
                        matchedContainers.Add(neighbour);
                    }
                }
            }

            if (container.HasSingleColor)
            {
                foreach (var matchedContainer in matchedContainers)
                {
                    var color = container.Colors.Peek();
                    Debug.Log("Color is " + color);


                    var block = matchedContainer.Pop();

                    while (true)
                    {
                        container.Push(block);

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

                        matchedContainer.Pop();
                    }
                }
            }
            else
            {
                
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