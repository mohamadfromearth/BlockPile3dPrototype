using System.Collections.Generic;
using System.Linq;
using Data;
using Event;
using Objects.AdvertiseBlock;
using Objects.Block;
using Objects.BlocksContainer;
using Objects.Cell;
using Objects.LockBlock;
using UnityEditor;
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
        [SerializeField] private CellColorAdderUI cellColorAdderUI;
        [SerializeField] private AddLockUI addLockUI;

        [SerializeField] private ColorRepository colorRepository;

        private Dictionary<Vector3Int, ICell> _cells;
        private ICell _selectedCell;
        private Color _cellColor;
        private Dictionary<Vector3Int, ILockBlock> _lockBlocks = new();
        private Dictionary<Vector3Int, IAdvertiseBlock> _advertiseBlocks = new();

        private List<string> _colors = new();

        private LevelDataSo _levelData;


        public void SaveBoard()
        {
            var path = "Assets/Data/Levels/Level";
            var levelIndexKey = "LEVEL_INDEX_KEY";
            var levelIndex = PlayerPrefs.GetInt(levelIndexKey, 1);
            path += levelIndex;
            path += ".asset";


            _levelData.size = _board.Width;
            _levelData.colors = _colors;
            _levelData.blockContainerDataList = GetBlockContainerDataList();
            _levelData.emptyHoldersPosList = GetEmptyPosList();
            _levelData.advertiseBlocks = GetAdvertiseBlocks();
            _levelData.lockBlocks = GetLockBlockDataList();
            _levelData.targetScore = designerUI.GetTargetScore();

            AssetDatabase.CreateAsset(_levelData, path);
            AssetDatabase.SaveAssets();

            _levelData = ScriptableObject.CreateInstance<LevelDataSo>();
        }

        private List<BlockContainerData> GetBlockContainerDataList()
        {
            var blockContainers = FindObjectsOfType<BlockContainer>();

            var blockContainerDataList = new List<BlockContainerData>();

            foreach (var blockContainer in blockContainers)
            {
                var blockContainerData = new BlockContainerData();
                blockContainerData.position = _board.WorldToCell(blockContainer.GetPosition());
                blockContainerData.color = blockContainer.Colors.ToList();

                blockContainerDataList.Add(blockContainerData);
            }

            return blockContainerDataList;
        }

        private List<Vector3Int> GetEmptyPosList()
        {
            var emptyPosList = new List<Vector3Int>();

            var cells = FindObjectsOfType<DefaultCell>();

            foreach (var cell in cells)
            {
                if (cell.GetColor() == Color.red)
                {
                    emptyPosList.Add(_board.WorldToCell(cell.GetPosition()));
                }
            }

            return emptyPosList;
        }

        private List<Vector3Int> GetAdvertiseBlocks()
        {
            var advertiseBlocksPos = new List<Vector3Int>();

            var advertiseBlocks = FindObjectsOfType<AdvertiseBlock>();

            foreach (var advertiseBlock in advertiseBlocks)
            {
                advertiseBlocksPos.Add(_board.WorldToCell(advertiseBlock.GetPosition()));
            }

            return advertiseBlocksPos;
        }

        private List<LockBlockData> GetLockBlockDataList()
        {
            var lockBlockDataList = new List<LockBlockData>();
            var lockBlocks = FindObjectsOfType<LockBlock>();

            foreach (var lockBlock in lockBlocks)
            {
                lockBlockDataList.Add(new LockBlockData(
                    _board.WorldToCell(lockBlock.GetPosition()),
                    lockBlock.Count
                ));
            }

            return lockBlockDataList;
        }


        private void Start()
        {
            _levelData = ScriptableObject.CreateInstance<LevelDataSo>();
            colorAdderUI.SetColors(colorRepository.colorsNames);
            cellColorAdderUI.SetColors(colorRepository.colorsNames);
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
            setUpCellUI.AddCancelClick(CancelSetUpCellUI);
            setUpCellUI.AddSetAsLeftEdgeClickListener(OnSetAsLeftEdge);
            setUpCellUI.AddSetAsLeftEdgeClickListener(OnSetAsRightEdge);

            colorAdderUI.AddCreateClickListener(OnCreateColor);

            addLockUI.AddCreateClickListener(OnCreateLock);

            cellColorAdderUI.AddAddClickListener(OnColorAddToCell);
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
            setUpCellUI.RemoveCancelClick(CancelSetUpCellUI);
            setUpCellUI.RemoveSetAsRightEdgeClickListener(OnSetAsRightEdge);
            setUpCellUI.RemoveSetAsRightEdgeClickListener(OnSetAsLeftEdge);

            colorAdderUI.RemoveCreateClickListener(OnCreateColor);

            addLockUI.RemoveCreateClickListener(OnCreateLock);

            cellColorAdderUI.RemoveAddClickListener(OnColorAddToCell);
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
            cellColorAdderUI.Show();
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


        private void OnColorAddToCell()
        {
            var color = colorRepository.GetColor(cellColorAdderUI.GetColorIndex());
            var count = cellColorAdderUI.GetColorsCount();

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

            cellColorAdderUI.Hide();
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


        private void CancelSetUpCellUI()
        {
            setUpCellUI.Hide();
        }


        public void OnAddColor()
        {
            setUpCellUI.Hide();
            colorAdderUI.Show();
        }

        public void RemoveColor() => designerUI.RemoveColor();


        private void OnCreateColor()
        {
            colorAdderUI.Hide();
            var colorName = colorRepository.GetColorName(colorAdderUI.GetValue());
            _colors.Add(colorName);
            designerUI.AddColor(colorName);
        }


        private void OnSetAsLeftEdge()
        {
            _levelData.leftEdgePosition = _board.WorldToCell(_selectedCell.GetPosition());
        }

        private void OnSetAsRightEdge()
        {
            _levelData.rightEdgePosition = _board.WorldToCell(_selectedCell.GetPosition());
        }

        #endregion
    }
}