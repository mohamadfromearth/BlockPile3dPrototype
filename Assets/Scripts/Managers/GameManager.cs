using System.Collections;
using System.Collections.Generic;
using Data;
using Event;
using Objects.BlocksContainer;
using Objects.Cell;
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
        [SerializeField] private BoosterInfoUI boosterInfoUI;

        [SerializeField] private LayerMask groundLayerMask;

        [SerializeField] private GameManagerHelpers helpers;
        [SerializeField] private BlocksMatcher blocksMatcher;
        [SerializeField] private GameObject hammer;
        [SerializeField] private Vector3 hammerHitRotation;
        [SerializeField] private Vector3 hammerInitialRotation;
        [SerializeField] private Grid grid;

        [SerializeField] private float backToSelectionBarDuration = 0.3f;


        [Inject] private Board _board;
        [Inject] private EventChannel _channel;
        [Inject] private ILevelRepository _levelRepository;
        [Inject] private AbilityRepository _abilityRepository;
        [Inject] private CurrencyRepository _currencyRepository;
        [Inject] private IProgressRewardsRepository _progressRewardsRepository;
        [Inject] private BlockContainerSelectionBar _selectionBar;
        [Inject] private Placer _placer;
        [Inject] private CameraSizeSetter _cameraSizeSetter;
        [Inject] private ShuffleController _shuffleController;


        private int _selectionBarSelectedIndex;
        private IBlockContainer _selectedBlockContainer;

        private float _currentScore;
        private float _previousScore;


        private StateManager _stateManager;


        private void Awake()
        {
            _camera = Camera.main;

            Application.targetFrameRate = 60;

            _stateManager = new StateManager(this);
        }


        private void Start()
        {
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
            _channel.Subscribe<TargetBlockDestroyed>(OnTargetBlockDestroyed);

            winUI.AddNextLevelClickListener(OnNextLevel);
            winUI.AddAdvertiseRewardClickListener(OnWinRewardAdvertiseClick);
            loseUI.AddRetryClickListener(OnRetry);
            loseUI.AddCoinReviveClickListener(OnLosingCoinRevive);
            loseUI.AddAdvertiseReviveClickListener(OnLosingAdvertiseRevive);

            gameUI.AddPunchClickListener(OnPunch);
            gameUI.AddSwapButtonClickListener(OnSwap);
            gameUI.AddRefreshButtonClickListener(OnRefreshSelectionBar);
            gameUI.AddBuyAbilityClickListener(OnBuyAbility);
            gameUI.AddWatchAdForAbilityClickListener(OnWatchAdToGetAbility);
            gameUI.AddAbilityCancelClickListener(OnAbilityCancel);
            gameUI.AddBlockToProgressAnimationFinishListener(OnBlockToProgressAnimationFinished);
            gameUI.AddBuyingAbilityCancelClickListener(OnAbilityBuyingCancel);
            gameUI.AddTargetGoalAnimationCompleted(OnTargetGoalUIAnimationCompleted);

            boosterInfoUI.AddClaimClickListener(OnClaimBooster);
        }

        private void UnSubscribeToEvents()
        {
            _channel.UnSubscribe<CellContainerPointerDown>(OnCellContainerPointerDown);
            _channel.UnSubscribe<CellContainerPointerUp>(OnCellContainerPointerUp);
            _channel.UnSubscribe<BlockDestroy>(OnBlocksDestroyed);
            _channel.UnSubscribe<UpdateBoardCompleted>(OnUpdateBoardCompleted);
            _channel.UnSubscribe<AdvertiseBlockPointerDown>(OnAdvertiseBlockPointerDown);
            _channel.UnSubscribe<ScoreHitLockBLock>(OnScoreHitLockBlock);
            _channel.UnSubscribe<TargetBlockDestroyed>(OnTargetBlockDestroyed);


            winUI.RemoveNextLevelClickListener(OnNextLevel);
            winUI.RemoveAdvertiseRewardClickListener(OnWinRewardAdvertiseClick);
            loseUI.RemoveRetryClickListener(OnRetry);
            loseUI.RemoveCoinReviveClickListener(OnLosingCoinRevive);
            loseUI.RemoveCoinReviveClickListener(OnLosingAdvertiseRevive);


            gameUI.RemovePunchClickListener(OnPunch);
            gameUI.RemoveSwapButtonClickListener(OnSwap);
            gameUI.RemoveRefreshButtonClickListener(OnRefreshSelectionBar);
            gameUI.RemoveBuyAbilityClickListener(OnBuyAbility);
            gameUI.RemoveWatchAdForAbilityClickListener(OnWatchAdToGetAbility);
            gameUI.RemoveAbilityCancelButtonListener(OnAbilityCancel);
            gameUI.RemoveBlockToProgressAnimationFinishListener(OnBlockToProgressAnimationFinished);
            gameUI.RemoveBuyingAbilityCancelClickListener(OnAbilityBuyingCancel);
            gameUI.RemoveTargetGoalAnimationCompleted(OnTargetGoalUIAnimationCompleted);


            boosterInfoUI.RemoveClaimClickListener(OnClaimBooster);
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
        }

        private void OnTargetBlockDestroyed()
        {
            var data = _channel.GetData<TargetBlockDestroyed>();

            gameUI.ShowBlockToProgressAnimation(data.Position);

            _previousScore = _currentScore;
            _currentScore += data.Count;
            _channel.Rise<ScoreChanged>(new ScoreChanged(_currentScore));
        }


        private void OnBlockToProgressAnimationFinished()
        {
            gameUI.SetProgress(_currentScore / _levelRepository.GetLevelData().targetScore);
            gameUI.AnimateProgressText((int)_previousScore, (int)_currentScore,
                "/" + _levelRepository.GetLevelData().targetScore);
        }

        public void OnNextLevel()
        {
            blocksMatcher.Stop();
            _board.Clear();
            _selectionBar.Clear();
            winUI.Hide();
            StartLevel();
            helpers.UpdateAbilityButtons(gameUI);
        }


        private void OnWinRewardAdvertiseClick()
        {
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
            if (blocksMatcher.AreBlocksMatching()) return;
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
            if (blocksMatcher.AreBlocksMatching()) return;
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

        private void OnLosingCoinRevive()
        {
            loseUI.Hide();
            _stateManager.ChangeState(GameStateType.GetAnotherChance);
        }

        private void OnLosingAdvertiseRevive()
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
                gameUI.SetCoinText(_currencyRepository.GetCoin().ToString());
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

        private void OnAbilityBuyingCancel()
        {
            gameUI.HideBuyingAbilityDialog();
        }

        public void OnShuffle() => _stateManager.Shuffle();


        private void OnTargetGoalUIAnimationCompleted()
        {
            var abilityData = _abilityRepository.GetAbilityData(_levelRepository.LevelIndex);
            if (abilityData != null)
            {
                boosterInfoUI.Show(abilityData);
            }
        }


        private void OnClaimBooster()
        {
            boosterInfoUI.Hide();
            var abilityData = _abilityRepository.GetAbilityData(_levelRepository.LevelIndex);
            _abilityRepository.UnLockAbility(abilityData);
            _abilityRepository.AddAbility(abilityData.type, 1);
            helpers.UpdateAbilityButtons(gameUI);
        }

        #endregion


        private bool CheckWin()
        {
            var levelData = _levelRepository.GetLevelData();

            if (_currentScore >= levelData.targetScore)
            {
                _progressRewardsRepository.IncreaseIndex();

                winUI.Show("Level: " + (_levelRepository.LevelIndex + 1), _currentScore.ToString(),
                    _levelRepository.GetLevelData().coinReward.ToString(),
                    _levelRepository.GetLevelData().buildingItemReward.ToString(),
                    _progressRewardsRepository.SpinLevelIndex /
                    (float)_progressRewardsRepository.SpinLevelTarget
                );

                _channel.Rise<Won>(new Won());

                _levelRepository.NextLevel();
                _currentScore = 0;


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
                loseUI.Show("Level: " + (_levelRepository.LevelIndex + 1),
                    (_levelRepository.GetLevelData().targetScore - _currentScore).ToString());

                _channel.Rise<Lose>(new Lose());

                return true;
            }

            return false;
        }

        private void StartLevel()
        {
            var levelData = _levelRepository.GetLevelData();

            gameUI.ShowTargetGoal(
                "Level: " + (_levelRepository.LevelIndex + 1),
                levelData.targetScore.ToString()
            );

            _board.SpawnCells(levelData.emptyHoldersPosList, levelData.size, levelData.size);
            _placer.Place();
            gameUI.SetProgressText(helpers.GetTargetScoreString(_currentScore));
            gameUI.SetProgress(0);
            _cameraSizeSetter.RefreshSize();
            _selectionBar.Spawn(levelData.colors, _board.WorldToCell(new Vector3Int(0, 0, 0)));
        }

        #region States

        private class PunchState : IGameState
        {
            private GameManager _gameManager;
            public PunchState(GameManager gameManager) => _gameManager = gameManager;

            public void OnEnter()
            {
                _gameManager.gameUI.ShowAbilityHintButton(
                    _gameManager._abilityRepository.GetAbilityData(AbilityType.Punch));
                _gameManager.helpers.ChangeCameraToAbilitiesState();
            }

            public void OnExit()
            {
                _gameManager.gameUI.HideAbilityHintButton();
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

                _gameManager.helpers.ShowHammerAnimation(containerBlock, () =>
                {
                    _gameManager._board.AddBlockContainer(null, containerBlock.GetPosition());

                    _gameManager.StartCoroutine(
                        _gameManager.hammer.DeActiveObjectWithDelay(containerBlock.DestroyAll()));

                    _gameManager._abilityRepository.RemoveAbility(AbilityType.Punch, 1);

                    _gameManager._stateManager.ChangeState(GameStateType.Default);

                    _gameManager.helpers.UpdateAbilityButtons(_gameManager.gameUI);
                });
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
                _gameManager.gameUI.ShowAbilityHintButton(
                    _gameManager._abilityRepository.GetAbilityData(AbilityType.Swap));
                _gameManager.helpers.ChangeCameraToAbilitiesState();
            }

            public void OnExit()
            {
                _gameManager.gameUI.HideAbilityHintButton();
                if (_firstSelectedBlockContainer != null)
                {
                    var pos = _firstSelectedBlockContainer.GetPosition();
                    pos.y = 0;
                    _firstSelectedBlockContainer.MoveTo(pos, 0.2f);
                    _firstSelectedBlockContainer = null;
                }
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

                    var pos = _firstSelectedBlockContainer.GetPosition();
                    pos.y = 0.5f;

                    _firstSelectedBlockContainer.MoveTo(pos, 0.2f);


                    return;
                }

                if (container != _firstSelectedBlockContainer)
                {
                    _gameManager.StartCoroutine(SwapUpdateRoutine(container));
                }
            }

            public void Shuffle()
            {
            }

            private IEnumerator SwapUpdateRoutine(IBlockContainer container)
            {
                float moveDuration = 0.3f;

                var firstPos = _firstSelectedBlockContainer.GetPosition();
                firstPos.y = 0;
                var secondPos = container.GetPosition();
                var upperFirstPos = firstPos;
                upperFirstPos.y = 3f;
                _firstSelectedBlockContainer.MoveTo(upperFirstPos, moveDuration);
                container.MoveTo(firstPos, moveDuration);

                yield return new WaitForSeconds(moveDuration);

                _firstSelectedBlockContainer.MoveTo(secondPos, moveDuration);

                yield return new WaitForSeconds(moveDuration);


                _gameManager._board.AddBlockContainer(_firstSelectedBlockContainer, secondPos);
                _gameManager._board.AddBlockContainer(container, firstPos);

                _firstSelectedBlockContainer = null;

                _gameManager._abilityRepository.RemoveAbility(AbilityType.Swap, 1);

                _gameManager._stateManager.ChangeState(GameStateType.Default);

                _gameManager.helpers.UpdateAbilityButtons(_gameManager.gameUI);


                yield return _gameManager.blocksMatcher.UpdateBoardRoutine(_gameManager._board.WorldToCell(firstPos),
                    true);
                yield return _gameManager.blocksMatcher.UpdateBoardRoutine(_gameManager._board.WorldToCell(secondPos),
                    true);
            }
        }

        private class DefaultState : IGameState
        {
            private GameManager _gameManager;

            private float _previousX = float.NaN;

            private bool _isRotating = false;

            private ICell _currentCell;

            private ICell _cellToDrop = null;


            public DefaultState(GameManager gameManager)
            {
                _gameManager = gameManager;
                _gameManager._channel.Subscribe<PointerUp>(OnPointerUp);
            }

            public void OnEnter()
            {
                _gameManager.helpers.ChangeCameraToDefaultState();
                _gameManager.gameUI.Show();
            }


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
                var blockContainerPos = PositionConverters.ScreenToWorldPosition(position, _gameManager._camera,
                    _gameManager.groundLayerMask);
                if (blockContainerPos.HasValue)
                {
                    var pos = blockContainerPos.Value;
                    pos.y += 2;
                    _gameManager._selectedBlockContainer.SetPosition(pos);


                    pos.y = 0;
                    pos.x += 0.8f;
                    pos.z += 0.8f;


                    var cell = _gameManager._board.GetCell(_gameManager.helpers
                        .ModifyBlockContainerPositionForRotatedGrid(pos));

                    _cellToDrop?.SetSelected(false);

                    if (cell != null)
                    {
                        cell.SetSelected(true);
                        _cellToDrop = cell;
                    }
                }
            }

            private void OnPointerUp()
            {
                _previousX = float.NaN;
                _cellToDrop?.SetSelected(false);
                _cellToDrop = null;

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
                        _gameManager._selectionBar.BackToInitialPosition(_gameManager._selectedBlockContainer,
                            _gameManager.backToSelectionBarDuration);
                        _gameManager._selectedBlockContainer = null;
                    }
                }
            }

            public void Shuffle()
            {
                if (_isRotating || _gameManager.helpers.IsShuffling ||
                    _gameManager.blocksMatcher.AreBlocksMatching() || _gameManager._board.IsRotationSnapping) return;

                _gameManager.helpers.ShuffleBoard();
                _gameManager._shuffleController.Hide();
                _gameManager._channel.Rise<Shuffle>(new Shuffle());
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


                _gameManager.helpers.ShowHammerAnimation(containerBlock, () =>
                {
                    _gameManager._board.AddBlockContainer(null, containerBlock.GetPosition());

                    _gameManager.StartCoroutine(
                        _gameManager.hammer.DeActiveObjectWithDelay(containerBlock.DestroyAll()));

                    foreach (var blockContainer in blockContainers)
                    {
                        _gameManager._board.AddBlockContainer(null, blockContainer.GetPosition());
                        blockContainer.DestroyAll();
                    }

                    _gameManager._board.AddBlockContainer(null, containerBlock.GetPosition());

                    _gameManager._stateManager.ChangeState(GameStateType.Default);
                });

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