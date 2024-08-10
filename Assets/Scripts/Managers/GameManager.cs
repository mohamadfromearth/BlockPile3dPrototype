using System.Collections.Generic;
using System.Linq;
using Data;
using Event;
using Objects.BlocksContainer;
using Scrips.Event;
using Scrips.Utils;
using UI;
using UnityEngine;
using Utils;
using Zenject;

namespace Managers
{
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
        [Inject] private Placer _placer;
        [Inject] private CameraSizeSetter _cameraSizeSetter;


        private int _selectionBarSelectedIndex;
        private IBlockContainer _selectedBlockContainer;

        private float _currentScore;

        private IGameState _currentState;
        private DefaultState _defaultState;
        private DoAbilityState _doAbilityState;


        private void Awake()
        {
            _camera = Camera.main;

            Application.targetFrameRate = 60;

            _defaultState = new DefaultState(this);
            _doAbilityState = new DoAbilityState();
            _currentState = _defaultState;
        }


        private void Start()
        {
            _selectionBarCellContainerPosList = selectionBarCellContainerTransformList.Select(t => t.position).ToList();
            _selectionBar.Spawn(_levelRepository.GetLevelData().colors);

            var levelData = _levelRepository.GetLevelData();
            _board.SpawnCells(levelData.emptyHoldersPosList, levelData.size, levelData.size);

            //_blockContainersPlacer.Place();
            _placer.Place();


            gameUI.SetProgressText(helpers.GetTargetScoreString(_currentScore));
            gameUI.SetProgress(0);

            _cameraSizeSetter.RefreshSize();
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
            _channel.Subscribe<UpdateBoardCompleted>(CheckWin);
            _channel.Subscribe<AdvertiseBlockPointerDown>(OnAdvertiseBlockPointerDown);
            _channel.Subscribe<ScoreHitLockBLock>(OnScoreHitLockBlock);

            winUI.AddNextLevelClickListener(OnNextLevel);
        }

        private void UnSubscribeToEvents()
        {
            _channel.UnSubscribe<CellContainerPointerDown>(OnCellContainerPointerDown);
            _channel.UnSubscribe<CellContainerPointerUp>(OnCellContainerPointerUp);
            _channel.UnSubscribe<BlockDestroy>(OnBlocksDestroyed);
            _channel.UnSubscribe<UpdateBoardCompleted>(CheckWin);
            _channel.UnSubscribe<AdvertiseBlockPointerDown>(OnAdvertiseBlockPointerDown);
            _channel.UnSubscribe<ScoreHitLockBLock>(OnScoreHitLockBlock);


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

                var holder = _board.GetCell(pos);

                if (holder != null && holder.CanPlaceItem)
                {
                    _selectedBlockContainer.IsPlaced = true;
                    _selectedBlockContainer.SetPosition(holder.GetPosition());

                    _board.AddBlockContainer(_selectedBlockContainer, pos);

                    var boardPosition = _board.WorldToCell(holder.GetPosition());
                    blocksMatcher.StartMatchingPosition = boardPosition;
                    StartCoroutine(blocksMatcher.UpdateBoardRoutine(boardPosition, true));

                    _selectedBlockContainer = null;

                    _selectionBar.Decrease();

                    if (_selectionBar.Count == 0) _selectionBar.Spawn(_levelRepository.GetLevelData().colors);
                }
                else
                {
                    _selectedBlockContainer.SetPosition(_selectionBarCellContainerPosList[_selectionBarSelectedIndex]);
                    _selectedBlockContainer = null;
                }
            }
        }


        private void OnAdvertiseBlockPointerDown()
        {
            var data = _channel.GetData<AdvertiseBlockPointerDown>();
            // Todo Show ad first then destroy the ad block
            _board.AddAdvertiseBlock(null, data.AdvertiseBlock.GetPosition());
            data.AdvertiseBlock.Destroy();
        }

        private void OnBlocksDestroyed()
        {
            var data = _channel.GetData<BlockDestroy>();

            _currentScore += data.Count;

            gameUI.SetProgress(_currentScore / _levelRepository.GetLevelData().targetScore);
            gameUI.SetProgressText(helpers.GetTargetScoreString(_currentScore));

            _channel.Rise<ScoreChanged>(new ScoreChanged(_currentScore));
        }

        private void OnNextLevel()
        {
            _board.Clear();
            _levelRepository.NextLevel();
            var levelData = _levelRepository.GetLevelData();
            _board.SpawnCells(levelData.emptyHoldersPosList, levelData.size, levelData.size);
            _placer.Place();
            winUI.Hide();
            gameUI.SetProgressText(helpers.GetTargetScoreString(_currentScore));
            gameUI.SetProgress(0);
            _cameraSizeSetter.RefreshSize();
        }

        private void OnScoreHitLockBlock()
        {
            var lockBlock = _channel.GetData<ScoreHitLockBLock>().LockBlock;
            _board.AddLockBlock(null, lockBlock.GetPosition());
            lockBlock.Destroy();
        }

        public void OnPointerMove(Vector2 position) => _currentState.OnPointerMove(position);

        #endregion


        private void CheckWin()
        {
            if (_currentScore >= _levelRepository.GetLevelData().targetScore)
            {
                _currentScore = 0;
                winUI.Show();
            }
        }


        #region States

        private class DoAbilityState : IGameState
        {
            public void OnPointerMove(Vector3 position)
            {
                return;
            }
        }

        private class DefaultState : IGameState
        {
            private GameManager _gameManager;

            public DefaultState(GameManager gameManager) => _gameManager = gameManager;

            public void OnPointerMove(Vector3 position)
            {
                if (_gameManager._selectedBlockContainer != null)
                {
                    var cellPos = PositionConverters.ScreenToWorldPosition(position, _gameManager._camera,
                        _gameManager.groundLayerMask);
                    if (cellPos != null)
                    {
                        _gameManager._selectedBlockContainer.SetPosition(cellPos.Value);
                    }
                }
            }
        }

        #endregion
    }
}