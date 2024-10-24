using System.Collections.Generic;
using Data;
using Event;
using Objects.AdvertiseBlock;
using Objects.Block;
using Objects.BlocksContainer;
using Objects.Cell;
using Objects.LockBlock;
using Objects.NoneValueLockBlock;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;
using LockBlock = Objects.LockBlock.LockBlock;

namespace Di
{
    public class GameInstaller : MonoInstaller
    {
        #region Prefabs

        [Header("Prefabs")] [SerializeField] private Block blockPrefab;
        [SerializeField] private Block[] blockPrefabs;
        [SerializeField] private BlockContainer blockContainerPrefab;

        [FormerlySerializedAs("cellPrefab")] [FormerlySerializedAs("blockContainerHolderPrefab")] [SerializeField]
        private DefaultCell defaultCellPrefab;

        [SerializeField] private LockBlock lockBlockPrefab;
        [SerializeField] private AdvertiseBlock advertiseBlockPrefab;
        [SerializeField] private NoneValueLockBlock noneValueLockBlockPrefab;

        #endregion

        [Header("Header")] [SerializeField] private LevelRepository levelRepository;
        [SerializeField] private ColorRepository colorRepository;
        [SerializeField] private BoardDataList boardData;
        [SerializeField] private List<Color> levelColors;
        [SerializeField] private MainRepository mainRepository;
        [SerializeField] private AbilityRepository abilityRepository;
        [SerializeField] private CurrencyRepository currencyRepository;
        [SerializeField] private ProgressRewardsRepository progressRewardsRepository;

        [SerializeField] private Grid grid;
        [SerializeField] private Transform gridPivot;

        // selectionBar
        [SerializeField] private List<Color> colors;
        [SerializeField] private BlockContainerSelectionBarData blockContainerSelectionBarData;

        [SerializeField] private Camera camera;
        [SerializeField] private CameraSizeSetterData cameraSizeSetterData;

        [SerializeField] private SettingsController settingsController;

        [SerializeField] private Transform shuffleButtonTransform;


        #region Tutorial

        [SerializeField] private GameObject tutorialIndicator;
        [SerializeField] private GameObject tutorialArrow;
        [SerializeField] private TextMeshProUGUI tutorialHintText;
        [SerializeField] private TutorialRepository tutorialRepository;

        #endregion


        public override void InstallBindings()
        {
            settingsController.Init();

            #region Core

            Container.Bind<EventChannel>().AsSingle().NonLazy();

            #endregion

            #region Factories

            Container.Bind<IBlockFactory>().To<BlockFactory>().AsSingle().WithArguments(blockPrefabs);

            Container.Bind<ICellFactory>().To<CellFactory>().AsSingle()
                .WithArguments(defaultCellPrefab, grid.transform).NonLazy();

            Container.Bind<IBlockContainerFactory>().To<BlockContainerFactory>().AsSingle()
                .WithArguments(blockContainerPrefab);

            Container.Bind<ILockBlockFactory>().To<LockBlockFactory>().AsSingle().WithArguments(lockBlockPrefab);


            Container.Bind<IAdvertiseBlockFactory>().To<AdvertiseBlockFactory>().AsSingle()
                .WithArguments(advertiseBlockPrefab);

            Container.Bind<INoneValueLockBlockFactory>().To<NoneValueLockBlockFactory>().AsSingle()
                .WithArguments(noneValueLockBlockPrefab);

            Container.Bind<TutorialCommand1Factory>().AsSingle()
                .WithArguments(tutorialRepository, tutorialIndicator, tutorialArrow, tutorialHintText);

            #endregion

            #region Repositories

            Container.Bind<ColorRepository>().FromInstance(colorRepository).AsSingle();

            //s  Container.Bind<LevelGenerator>().AsSingle().WithArguments(boardData, levelColors);


            Container.Bind<ILevelRepository>().FromInstance(levelRepository).AsSingle();

            Container.Bind<MainRepository>().FromInstance(mainRepository).AsSingle();

            Container.Bind<AbilityRepository>().FromInstance(abilityRepository).AsSingle();

            Container.Bind<CurrencyRepository>().FromInstance(currencyRepository).AsSingle();

            Container.Bind<IProgressRewardsRepository>().FromInstance(progressRewardsRepository).AsSingle();

            #endregion

            var levelData = levelRepository.GetLevelData();
            Container.Bind<Board>().AsSingle().WithArguments(levelData.size, levelData.size, grid, gridPivot);

            Container.Bind<BlockContainerSelectionBar>().AsSingle()
                .WithArguments(blockContainerSelectionBarData);

            #region Placers

            Container.Bind<BlockContainersPlacer>().AsTransient();
            Container.Bind<AdvertiseBlockPlacer>().AsTransient();
            Container.Bind<LockBlockPlacer>().AsTransient();
            Container.Bind<Placer>().AsTransient();

            #endregion

            #region Helpers

            Container.Bind<CameraSizeSetter>().AsTransient().WithArguments(cameraSizeSetterData);

            Container.Bind<ShuffleController>().AsSingle().WithArguments(shuffleButtonTransform).NonLazy();

            #endregion
        }
    }
}