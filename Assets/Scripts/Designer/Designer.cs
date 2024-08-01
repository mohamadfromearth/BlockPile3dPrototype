using System.Collections.Generic;
using Data;
using Event;
using Objects.AdvertiseBlock;
using Objects.Block;
using Objects.Cell;
using Objects.LockBlock;
using Scrips.Objects.Cell;
using Scrips.Objects.CellsContainer;
using UnityEngine;
using Zenject;
using LockBlock = Objects.LockBlock.LockBlock;

namespace Designer
{
    public class Designer : MonoBehaviour
    {
        [Inject] private Board _board;
        [Inject] private EventChannel _channel;
        [Inject] private IBlockContainerFactory _blockContainerFactory;
        [Inject] private IBlockFactory _blockFactory;
        [Inject] private ILockBlockFactory _lockBlockFactory;
        [Inject] private IAdvertiseBlockFactory _advertiseBlockFactory;


        [SerializeField] private DesignerUI designerUI;
        [SerializeField] private BoardSetUpUI boardSetUpUI;
        [SerializeField] private SetUpCellUI setUpCellUI;
        [SerializeField] private ColorAdderUI colorAdderUI;
        [SerializeField] private AddLockUI addLockUI;

        [SerializeField] private ColorRepository colorRepository;

        private Dictionary<Vector3Int, ICell> _cells;
        private ICell _selectedCell;
        private Color _cellColor;
        private Dictionary<Vector3Int, ILockBlock> _lockBlocks = new();
        private Dictionary<Vector3Int, IAdvertiseBlock> _advertiseBlocks = new();


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
            setUpCellUI.AddAddCellClickListener(OnAddCell);
            setUpCellUI.AddAddAdvertiseClickListener(OnAddAdvertise);
            setUpCellUI.AddRemoveAdvertiseClickListener(OnRemoveAdvertise);
            setUpCellUI.AddAddLockClickListener(OnAddLock);
            setUpCellUI.AddRemoveLockClickListener(OnRemoveLock);

            addLockUI.AddCreateClickListener(OnCreateLock);

            colorAdderUI.AddAddClickListener(OnColorAdderAdd);
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
            setUpCellUI.RemoveAddCellClickListener(OnAddCell);
            setUpCellUI.RemoveAddAdvertiseClickListener(OnAddAdvertise);
            setUpCellUI.RemoveRemoveAdvertiseClickListener(OnRemoveAdvertise);
            setUpCellUI.RemoveAddLockClickListener(OnAddLock);
            setUpCellUI.RemoveRemoveLockClickListener(OnRemoveLock);

            addLockUI.RemoveCreateClickListener(OnCreateLock);

            colorAdderUI.RemoveAddClickListener(OnColorAdderAdd);
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
            _cellColor = _selectedCell.GetColor();
            _selectedCell.SetColor(Color.red);
            setUpCellUI.Hide();

            if (_selectedCell.BlockContainer != null)
            {
                _selectedCell.BlockContainer.Destroy(true);
                _selectedCell.BlockContainer = null;
            }
        }


        private void OnAddCell()
        {
            _selectedCell.SetColor(_cellColor);
            setUpCellUI.Hide();
        }

        private void OnAddColorToCell()
        {
            setUpCellUI.Hide();
            colorAdderUI.Show();
        }

        private void OnRemoveColorFromCell()
        {
            setUpCellUI.Hide();

            var container = _selectedCell.BlockContainer;
            var colorsCount = container.Colors.Count;

            while (colorsCount == container.Colors.Count)
            {
                container.Pop().Destroy();
            }

            if (container.Colors.Count == 0)
            {
                container.Destroy();
                _selectedCell.BlockContainer = null;
            }
        }


        private void OnColorAdderAdd()
        {
            var color = colorRepository.GetColor(colorAdderUI.GetColorIndex());
            var count = colorAdderUI.GetColorsCount();

            if (_selectedCell.BlockContainer == null)
            {
                _selectedCell.BlockContainer = _blockContainerFactory.Create();
                _selectedCell.BlockContainer.SetPosition(_selectedCell.GetPosition());
            }

            _selectedCell.BlockContainer.GameObj.GetComponent<BoxCollider>().enabled = false;

            for (int i = 0; i < count; i++)
            {
                var block = _blockFactory.Create();
                block.Color = color;
                _selectedCell.BlockContainer.Push(block);
            }

            colorAdderUI.Hide();
        }


        private void OnAddAdvertise()
        {
            var advertiseBlock = _advertiseBlockFactory.Create();
            advertiseBlock.SetPosition(_selectedCell.GetPosition());
            _advertiseBlocks[_board.WorldToCell(advertiseBlock.GetPosition())] = advertiseBlock;
            
            setUpCellUI.Hide();
        }

        private void OnRemoveAdvertise()
        {
            var gridPos = _board.WorldToCell(_selectedCell.GetPosition());

            if (_advertiseBlocks.TryGetValue(gridPos, out var advertiseBlock))
            {
                advertiseBlock.Destroy();
                _advertiseBlocks.Remove(gridPos);
            }

            setUpCellUI.Hide();
        }


        private void OnAddLock()
        {
            setUpCellUI.Hide();
            addLockUI.Show();
        }

        private void OnRemoveLock()
        {
            var gridPos = _board.WorldToCell(_selectedCell.GetPosition());

            if (_lockBlocks.TryGetValue(gridPos, out var lockBlock))
            {
                lockBlock.Destroy();
                _lockBlocks.Remove(gridPos);
            }


            setUpCellUI.Hide();
        }

        private void OnCreateLock()
        {
            var lockBlock = _lockBlockFactory.Create();
            lockBlock.SetPosition(_selectedCell.GetPosition());
            lockBlock.Count = addLockUI.GetCount();
            _lockBlocks[_board.WorldToCell(_selectedCell.GetPosition())] = lockBlock;
            addLockUI.Hide();
        }

        #endregion
    }
}