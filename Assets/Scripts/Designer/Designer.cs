using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Event;
using Objects.Cell;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Designer
{
    public class Designer : MonoBehaviour
    {
        [Inject] private Board _board;
        [Inject] private EventChannel _channel;

        [SerializeField] private Color cellColor;

        [SerializeField] private DesignerUI designerUI;
        [SerializeField] private BoardSetUpUI boardSetUpUI;
        [SerializeField] private SetUpCellUI setUpCellUI;

        private Dictionary<Vector3Int, ICell> _cells;
        private ICell _selectedCell;


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
            _channel.Subscribe<CellClick>(OnCellClick);

            designerUI.AddSetUpBoardClickListener(OnSetUpBoard);
            boardSetUpUI.AddGenerateClickListener(OnGenerate);
            boardSetUpUI.AddCancelListener(OnSetUpBoardCancel);

            setUpCellUI.AddRemoveCellClickListener(OnRemoveCell);
            setUpCellUI.AddAddColorClickListener(OnAddColorToCell);
            setUpCellUI.AddRemoveColorClickListener(OnRemoveColorFromCell);
        }


        private void UnSubscribeToEvents()
        {
            _channel.UnSubscribe<CellClick>(OnCellClick);

            designerUI.RemoveSetUpBoardClickListener(OnSetUpBoard);
            boardSetUpUI.RemoveGenerateClickListener(OnGenerate);
            boardSetUpUI.RemoveCancelListener(OnSetUpBoardCancel);

            setUpCellUI.RemoveRemoveCellClickListener(OnRemoveCell);
            setUpCellUI.RemoveAddColorClickListener(OnAddColorToCell);
            setUpCellUI.RemoveRemoveColorClickListener(OnRemoveColorFromCell);
        }


        #region subscribers

        private void OnGenerate()
        {
            var size = boardSetUpUI.GetSize();

            _cells = _board.SpawnCells(new List<Vector3Int>(), size, size);

            boardSetUpUI.Hide();
        }

        private void OnSetUpBoard() => boardSetUpUI.Show();

        private void OnSetUpBoardCancel() => boardSetUpUI.Hide();


        private void OnCellClick()
        {
            var data = _channel.GetData<CellClick>();
            _selectedCell = data.Cell;
            setUpCellUI.Show();
        }


        private void OnRemoveCell()
        {
            _selectedCell.SetColor(Color.red);
        }


        private void OnAddCell()
        {
            _selectedCell.SetColor(cellColor);
        }

        private void OnAddColorToCell()
        {
        }

        private void OnRemoveColorFromCell()
        {
        }

        #endregion
    }
}