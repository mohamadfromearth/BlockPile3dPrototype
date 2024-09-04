using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using Event;
using Objects.BlocksContainer;
using Scrips.Event;
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
        [SerializeField] private LoseUI loseUI;

        [SerializeField] private LayerMask groundLayerMask;

        [SerializeField] private List<Transform> selectionBarCellContainerTransformList;
        private List<Vector3> _selectionBarCellContainerPosList = new();
        [SerializeField] private GameManagerHelpers helpers;
        [SerializeField] private BlocksMatcher blocksMatcher;
        [SerializeField] private Grid grid;

        [Inject] private Board _board;
        [Inject] private EventChannel _channel;
        [Inject] private ILevelRepository _levelRepository;
        [Inject] private AbilityRepository _abilityRepository;
        [Inject] private CurrencyRepository _currencyRepository;
        [Inject] private BlockContainerSelectionBar _selectionBar;
        [Inject] private Placer _placer;
        [Inject] private CameraSizeSetter _cameraSizeSetter;


        private int _selectionBarSelectedIndex;
        private IBlockContainer _selectedBlockContainer;

        private float _currentScore;


        private StateManager _stateManager;


        private void Awake()
        {
            _camera = Camera.main;

            Application.targetFrameRate = 60;

            _stateManager = new StateManager(this);
        }


        private void Start()
        {
            _selectionBarCellContainerPosList = selectionBarCellContainerTransformList.Select(t => t.position).ToList();

            StartLevel();

            helpers.UpdateAbilityButtons(gameUI);
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
            _channel.Subscribe<UpdateBoardCompleted>(OnUpdateBoardCompleted);
            _channel.Subscribe<AdvertiseBlockPointerDown>(OnAdvertiseBlockPointerDown);
            _channel.Subscribe<ScoreHitLockBLock>(OnScoreHitLockBlock);

            winUI.AddNextLevelClickListener(OnNextLevel);
            loseUI.AddRetryClickListener(OnRetry);
            loseUI.AddGetChanceClickListener(OnGetAnotherChance);

            gameUI.AddPunchClickListener(OnPunch);
            gameUI.AddSwapButtonClickListener(OnSwap);
            gameUI.AddRefreshButtonClickListener(OnRefreshSelectionBar);
            gameUI.AddBuyAbilityClickListener(OnBuyAbility);
            gameUI.AddWatchAdForAbilityClickListener(OnWatchAdToGetAbility);
            gameUI.AddAbilityCancelClickListener(OnAbilityCancel);
        }

        private void UnSubscribeToEvents()
        {
            _channel.UnSubscribe<CellContainerPointerDown>(OnCellContainerPointerDown);
            _channel.UnSubscribe<CellContainerPointerUp>(OnCellContainerPointerUp);
            _channel.UnSubscribe<BlockDestroy>(OnBlocksDestroyed);
            _channel.UnSubscribe<UpdateBoardCompleted>(OnUpdateBoardCompleted);
            _channel.UnSubscribe<AdvertiseBlockPointerDown>(OnAdvertiseBlockPointerDown);
            _channel.UnSubscribe<ScoreHitLockBLock>(OnScoreHitLockBlock);


            winUI.RemoveNextLevelClickListener(OnNextLevel);
            loseUI.RemoveRetryClickListener(OnRetry);
            loseUI.RemoveGetChanceClickListener(OnGetAnotherChance);

            gameUI.RemovePunchClickListener(OnPunch);
            gameUI.RemoveSwapButtonClickListener(OnSwap);
            gameUI.RemoveRefreshButtonClickListener(OnRefreshSelectionBar);
            gameUI.RemoveBuyAbilityClickListener(OnBuyAbility);
            gameUI.RemoveWatchAdForAbilityClickListener(OnWatchAdToGetAbility);
            gameUI.RemoveAbilityCancelButtonListener(OnAbilityCancel);
        }


        #region Subscribers

        private void OnCellContainerPointerDown() => _stateManager.OnContainerPointerDown();


        private void OnCellContainerPointerUp() => _stateManager.OnContainerPointerUp();


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
            _selectionBar.Clear();
            _levelRepository.NextLevel();
            winUI.Hide();
            StartLevel();
            helpers.UpdateAbilityButtons(gameUI);
        }


        private void OnScoreHitLockBlock()
        {
            var lockBlock = _channel.GetData<ScoreHitLockBLock>().LockBlock;
            _board.AddLockBlock(null, lockBlock.GetPosition());
            lockBlock.Destroy();
        }

        private void OnUpdateBoardCompleted
            ()
        {
            if (CheckWin()) return;
            CheckLose();
        }

        public void OnPointerMove(Vector2 position) => _stateManager.OnPointerMove(position);

        public void OnPointerUp()
        {
        }


        private void OnPunch()
        {
            if (_abilityRepository.GetAbilityData(AbilityType.Punch).count == 0)
            {
                gameUI.ShowBuyingAbilityDialog(_abilityRepository.GetAbilityData(AbilityType.Punch));
            }
            else
            {
                _stateManager.ChangeState(GameStateType.Punch);
            }
        }

        private void OnSwap()
        {
            if (_abilityRepository.GetAbilityData(AbilityType.Swap).count == 0)
            {
                gameUI.ShowBuyingAbilityDialog(_abilityRepository.GetAbilityData(AbilityType.Swap));
            }
            else
            {
                _stateManager.ChangeState(GameStateType.Swap);
            }
        }


        private void OnRefreshSelectionBar()
        {
            if (_abilityRepository.GetAbilityData(AbilityType.Refresh).count == 0)
            {
                gameUI.ShowBuyingAbilityDialog(_abilityRepository.GetAbilityData(AbilityType.Refresh));
            }
            else
            {
                _selectionBar.Refresh(_levelRepository.GetLevelData().colors);
                _abilityRepository.RemoveAbility(AbilityType.Refresh, 1);
                helpers.UpdateAbilityButtons(gameUI);
            }
        }

        private void OnRetry()
        {
            _board.Clear();
            _selectionBar.Clear();
            StartLevel();
            loseUI.Hide();
        }

        private void OnGetAnotherChance()
        {
            loseUI.Hide();
            _stateManager.ChangeState(GameStateType.GetAnotherChance);
        }


        private void OnAbilityCancel() => _stateManager.ChangeState(GameStateType.Default);

        private void OnBuyAbility()
        {
            if (_currencyRepository.GetCoin() >= gameUI.AbilityData.cost)
            {
                _abilityRepository.AddAbility(gameUI.AbilityData.type, 1);
                helpers.UpdateAbilityButtons(gameUI);
                _stateManager.ChangeState(gameUI.AbilityData.type.GameStateType());
                _currencyRepository.RemoveCoin(gameUI.AbilityData.cost);
                gameUI.SetCoinText("Coin" + _currencyRepository.GetCoin());
            }
            else
            {
                // TODO not implemented
            }

            gameUI.HideBuyingAbilityDialog();
        }

        private void OnWatchAdToGetAbility()
        {
            _abilityRepository.AddAbility(gameUI.AbilityData.type, 1);
            helpers.UpdateAbilityButtons(gameUI);
            _stateManager.ChangeState(gameUI.AbilityData.type.GameStateType());
            gameUI.HideBuyingAbilityDialog();
        }

        public void OnShuffle() => _stateManager.Shuffle();

        #endregion


        private bool CheckWin()
        {
            var levelData = _levelRepository.GetLevelData();

            if (_currentScore >= levelData.targetScore)
            {
                _currentScore = 0;
                winUI.Show();

                _currencyRepository.AddCoin(levelData.coinReward);
                gameUI.SetCoinText("Coin:" + _currencyRepository.GetCoin());

                return true;
            }

            return false;
        }

        private bool CheckLose()
        {
            if (_board.IsFilled)
            {
                loseUI.Show();
                return true;
            }

            return false;
        }

        private void StartLevel()
        {
            var levelData = _levelRepository.GetLevelData();
            _selectionBar.Spawn(levelData.colors);
            _board.SpawnCells(levelData.emptyHoldersPosList, levelData.size, levelData.size);
            _placer.Place();
            gameUI.SetProgressText(helpers.GetTargetScoreString(_currentScore));
            gameUI.SetProgress(0);
            _cameraSizeSetter.RefreshSize();
        }

        #region States

        private class PunchState : IGameState
        {
            private GameManager _gameManager;
            public PunchState(GameManager gameManager) => _gameManager = gameManager;

            public void OnEnter()
            {
                _gameManager.gameUI.ShowAbilityCancelButton();
            }

            public void OnExit()
            {
                _gameManager.gameUI.HideAbilityCancelButton();
            }

            public void OnPointerMove(Vector3 position)
            {
            }

            public void OnContainerPointerDown()
            {
            }


            public void OnContainerPointerUp()
            {
                var containerBlock = _gameManager._channel.GetData<CellContainerPointerUp>().BlockContainer;
                _gameManager._board.AddBlockContainer(null, containerBlock.GetPosition());
                containerBlock.Destroy(true);

                _gameManager._abilityRepository.RemoveAbility(AbilityType.Punch, 1);

                _gameManager._stateManager.ChangeState(GameStateType.Default);

                _gameManager.helpers.UpdateAbilityButtons(_gameManager.gameUI);
            }

            public void Shuffle()
            {
            }
        }

        private class SwapState : IGameState
        {
            private GameManager _gameManager;

            public SwapState(GameManager gameManager)
            {
                _gameManager = gameManager;
            }

            private IBlockContainer _firstSelectedBlockContainer = null;

            public void OnEnter()
            {
                _gameManager.gameUI.ShowAbilityCancelButton();
            }

            public void OnExit()
            {
                _gameManager.gameUI.HideAbilityCancelButton();
                _firstSelectedBlockContainer = null;
            }

            public void OnPointerMove(Vector3 position)
            {
            }

            public void OnContainerPointerDown()
            {
            }


            public void OnContainerPointerUp()
            {
                var container = _gameManager._channel.GetData<CellContainerPointerUp>().BlockContainer;

                if (container.IsPlaced == false) return;

                if (_firstSelectedBlockContainer == null)
                {
                    _firstSelectedBlockContainer = container;

                    return;
                }

                if (container != _firstSelectedBlockContainer)
                {
                    var firstPos = _firstSelectedBlockContainer.GetPosition();
                    var secondPos = container.GetPosition();
                    _firstSelectedBlockContainer.SetPosition(secondPos);
                    container.SetPosition(firstPos);
                    _gameManager._board.AddBlockContainer(_firstSelectedBlockContainer, secondPos);
                    _gameManager._board.AddBlockContainer(container, firstPos);

                    _gameManager.StartCoroutine(SwapUpdateRoutine(_gameManager._board.WorldToCell(firstPos),
                        _gameManager._board.WorldToCell(secondPos)));

                    _gameManager._abilityRepository.RemoveAbility(AbilityType.Swap, 1);

                    _gameManager._stateManager.ChangeState(GameStateType.Default);

                    _gameManager.helpers.UpdateAbilityButtons(_gameManager.gameUI);
                }
            }

            public void Shuffle()
            {
            }

            private IEnumerator SwapUpdateRoutine(Vector3Int firstPos, Vector3Int secondPos)
            {
                yield return _gameManager.blocksMatcher.UpdateBoardRoutine(firstPos, true);
                yield return _gameManager.blocksMatcher.UpdateBoardRoutine(secondPos, true);
            }
        }

        private class DefaultState : IGameState
        {
            private GameManager _gameManager;

            private float _previousX = float.NaN;

            private bool _isRotating = false;


            private Quaternion _initialRotation = Quaternion.identity;

            public DefaultState(GameManager gameManager)
            {
                _gameManager = gameManager;
                _gameManager._channel.Subscribe<PointerUp>(OnPointerUp);
            }

            public void OnEnter() => _gameManager.gameUI.Show();


            public void OnExit() => _gameManager.gameUI.Hide();


            public void OnPointerMove(Vector3 position)
            {
                if (_gameManager._selectedBlockContainer != null)
                {
                    MoveSelectedContainer(position);
                }
                else
                {
                    RotateBoard(position);
                }
            }

            public void OnContainerPointerDown()
            {
                var container = _gameManager._channel.GetData<CellContainerPointerDown>().BlockContainer;
                if (container.IsPlaced) return;

                _gameManager._selectedBlockContainer = container;

                _gameManager._selectionBarSelectedIndex =
                    ListUtils.GetIndex(_gameManager._selectedBlockContainer.GetPosition(),
                        _gameManager._selectionBarCellContainerPosList);
            }


            private void RotateBoard(Vector3 position)
            {
                if (_gameManager.blocksMatcher.AreBlocksMatching() || _gameManager.helpers.IsShuffling ||
                    _gameManager._board.IsRotationSnapping) return;

                if (float.IsNaN(_previousX))
                {
                    _previousX = position.x;
                    return;
                }

                if (_previousX == position.x) return;

                _isRotating = true;
                _gameManager._board.Rotate((position.x - _previousX) / 5f);
                _previousX = position.x;
            }

            private void MoveSelectedContainer(Vector3 position)
            {
                var cellPos = PositionConverters.ScreenToWorldPosition(position, _gameManager._camera,
                    _gameManager.groundLayerMask);
                if (cellPos.HasValue)
                {
                    var pos = cellPos.Value;
                    pos.y += 2;
                    _gameManager._selectedBlockContainer.SetPosition(pos);
                }
            }

            private void OnPointerUp()
            {
                _previousX = float.NaN;

                if (_gameManager._selectedBlockContainer == null &&
                    _gameManager.blocksMatcher.AreBlocksMatching() == false &&
                    _gameManager.helpers.IsShuffling == false && _isRotating)
                {
                    _gameManager._board.SnapRotation();
                }

                _isRotating = false;
            }

            public void OnContainerPointerUp()
            {
                if (_gameManager._selectedBlockContainer != null)
                {
                    var pos = _gameManager._selectedBlockContainer.GetPosition();
                    pos.y = 0;
                    pos.x += 0.8f;
                    pos.z += 0.8f;

                    var holder =
                        _gameManager._board.GetCell(
                            _gameManager.helpers.ModifyBlockContainerPositionForRotatedGrid(pos));

                    if (holder != null && holder.CanPlaceItem)
                    {
                        _gameManager._selectedBlockContainer.IsPlaced = true;
                        _gameManager._selectedBlockContainer.SetPosition(holder.GetPosition());

                        _gameManager._board.AddBlockContainer(_gameManager._selectedBlockContainer,
                            holder.GetPosition());
                        _gameManager._selectedBlockContainer.SetParent(_gameManager.grid.transform);


                        var boardPosition = _gameManager._board.WorldToCell(holder.GetPosition());


                        _gameManager.blocksMatcher.StartMatchingPosition = boardPosition;
                        _gameManager.StartCoroutine(_gameManager.blocksMatcher.UpdateBoardRoutine(boardPosition,
                            true));

                        _gameManager._selectedBlockContainer = null;

                        _gameManager._selectionBar.Decrease();

                        if (_gameManager._selectionBar.Count == 0)
                            _gameManager._selectionBar.Spawn(_gameManager._levelRepository.GetLevelData().colors);
                    }
                    else
                    {
                        _gameManager._selectedBlockContainer.SetPosition(
                            _gameManager._selectionBarCellContainerPosList[_gameManager._selectionBarSelectedIndex]);
                        _gameManager._selectedBlockContainer = null;
                    }
                }
            }

            public void Shuffle()
            {
                if (_isRotating || _gameManager.helpers.IsShuffling ||
                    _gameManager.blocksMatcher.AreBlocksMatching() || _gameManager._board.IsRotationSnapping) return;

                _gameManager.helpers.ShuffleBoard();
            }
        }

        private class GetAnotherChanceState : IGameState
        {
            private GameManager _gameManager;

            public GetAnotherChanceState(GameManager gameManager)
            {
                _gameManager = gameManager;
            }

            public void OnEnter()
            {
            }

            public void OnExit()
            {
            }

            public void OnPointerMove(Vector3 position)
            {
            }

            public void OnContainerPointerDown()
            {
            }

            public void OnContainerPointerUp()
            {
                var containerBlock = _gameManager._channel.GetData<CellContainerPointerUp>().BlockContainer;

                var gridPos = _gameManager._board.WorldToCell(containerBlock.GetPosition());

                var blockContainers = _gameManager.helpers.GetAnotherChanceBlocks(_gameManager._board, gridPos);

                foreach (var blockContainer in blockContainers)
                {
                    _gameManager._board.AddBlockContainer(null, blockContainer.GetPosition());
                    blockContainer.Destroy(true);
                }

                _gameManager._board.AddBlockContainer(null, containerBlock.GetPosition());
                containerBlock.Destroy(true);

                _gameManager._stateManager.ChangeState(GameStateType.Default);
            }

            public void Shuffle()
            {
            }
        }


        private class StateManager : IGameState
        {
            private Dictionary<GameStateType, IGameState> _gameStates;
            private IGameState _state;


            public StateManager(GameManager gameManager)
            {
                _gameStates = new Dictionary<GameStateType, IGameState>()
                {
                    { GameStateType.Punch, new PunchState(gameManager) },
                    { GameStateType.Default, new DefaultState(gameManager) },
                    { GameStateType.Swap, new SwapState(gameManager) },
                    { GameStateType.GetAnotherChance, new GetAnotherChanceState(gameManager) },
                };

                _state = _gameStates[GameStateType.Default];
            }

            public void ChangeState(GameStateType type)
            {
                OnExit();
                _state = _gameStates[type];
                OnEnter();
            }

            public void OnEnter() => _state.OnEnter();

            public void OnExit() => _state.OnExit();

            public void OnPointerMove(Vector3 position) => _state.OnPointerMove(position);
            public void OnContainerPointerDown() => _state.OnContainerPointerDown();

            public void OnContainerPointerUp() => _state.OnContainerPointerUp();
            public void Shuffle() => _state.Shuffle();
        }

        #endregion
    }
}