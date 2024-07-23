using System.Collections.Generic;
using System.Linq;
using Data;
using Event;
using Objects.Block;
using Objects.BlocksContainer;
using Objects.Cell;
using Scrips.Objects.Cell;
using Scrips.Objects.CellsContainer;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Di
{
    public class GameInstaller : MonoInstaller
    {
        // prefabs
        [SerializeField] private Block blockPrefab;
        [SerializeField] private BlockContainer blockContainerPrefab;

        [FormerlySerializedAs("cellPrefab")] [FormerlySerializedAs("blockContainerHolderPrefab")] [SerializeField]
        private DefaultCell defaultCellPrefab;

        // so
        [SerializeField] private LevelRepository levelRepository;
        [SerializeField] private BoardDataList boardData;
        [SerializeField] private List<Color> levelColors;

        [SerializeField] private Grid grid;

        // selectionBar
        [SerializeField] private List<Color> colors;
        [SerializeField] private List<Transform> selectionBarPositionList;

        [SerializeField] private Camera camera;

        public override void InstallBindings()
        {
            Container.Bind<EventChannel>().AsSingle().NonLazy();

            Container.Bind<IBlockFactory>().To<BlockFactory>().AsSingle().WithArguments(blockPrefab);

            Container.Bind<ICellFactory>().To<CellFactory>().AsSingle()
                .WithArguments(defaultCellPrefab).NonLazy();

            Container.Bind<IBlockContainerFactory>().To<BlockContainerFactory>().AsSingle()
                .WithArguments(blockContainerPrefab);

            Container.Bind<LevelGenerator>().AsSingle().WithArguments(boardData, levelColors);


            Container.Bind<ILevelRepository>().To<GenerativeLevelRepository>().AsSingle();


            var levelData = levelRepository.GetLevelData();
            Container.Bind<Board>().AsSingle().WithArguments(levelData.width, levelData.height, grid);

            Container.Bind<BlockContainerSelectionBar>().AsTransient()
                .WithArguments(selectionBarPositionList.Select(t => t.position).ToList());

            Container.Bind<BlockContainersPlacer>().AsTransient();

            Container.Bind<CameraSizeSetter>().AsTransient().WithArguments(camera);
        }
    }
}