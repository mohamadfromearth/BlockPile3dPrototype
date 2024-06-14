using System.Collections.Generic;
using System.Linq;
using Event;
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
        [Inject] private EventChannel _channel;

        private int selectionBarSelectedIndex;
        private ICellContainer selectedCellCotainer;


        private void Awake()
        {
            _camera = Camera.main;
        }


        private void Start()
        {
            _selectionBarCellContainerPosList = selectionBarCellContainerTransformList.Select(t => t.position).ToList();
            _helpers.SpawnCellContainers(_selectionBarCellContainerPosList);
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
            selectedCellCotainer = _channel.GetData<CellContainerPointerDown>().CellContainer;

            selectionBarSelectedIndex =
                ListUtils.GetIndex(selectedCellCotainer.GetPosition(), _selectionBarCellContainerPosList);
        }

        private void OnCellContainerPointerUp()
        {
            if (selectedCellCotainer != null)
            {
                selectedCellCotainer.SetPosition(_selectionBarCellContainerPosList[selectionBarSelectedIndex]);
                selectedCellCotainer = null;
            }
        }

        public void OnPointerMove(Vector2 position)
        {
            if (selectedCellCotainer != null)
            {
                var cellPos = PositionConverters.ScreenToWorldPosition(position, _camera, groundLayerMask);
                if (cellPos != null) selectedCellCotainer.SetPosition(cellPos.Value);
            }
        }

        #endregion
    }
}