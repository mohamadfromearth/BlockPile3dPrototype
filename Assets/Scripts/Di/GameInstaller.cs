using Event;
using Scrips.Data;
using Scrips.Objects.Block;
using Scrips.Objects.BlockContainerHolder;
using Scrips.Objects.BlocksContainer;
using Scrips.Objects.Cell;
using Scrips.Objects.CellsContainer;
using Scripts;
using Scripts.Data;
using Scripts.Objects.BlocksContainer;
using UnityEngine;
using Zenject;

namespace Scrips.Di
{
    public class GameInstaller : MonoInstaller
    {
        // prefabs
        [SerializeField] private Block blockPrefab;
        [SerializeField] private BlockContainer blockContainerPrefab;
        [SerializeField] private BlockContainerHolder blockContainerHolderPrefab;

        // so
        [SerializeField] private LevelRepository levelRepository;

        [SerializeField] private Grid grid;

        public override void InstallBindings()
        {
            Container.Bind<EventChannel>().AsSingle().NonLazy();

            Container.Bind<IBlockFactory>().To<BlockFactory>().AsSingle().WithArguments(blockPrefab);

            Container.Bind<IBlockContainerHolderFactory>().To<BlockContainerHolderFactory>().AsSingle()
                .WithArguments(blockContainerHolderPrefab).NonLazy();

            Container.Bind<IBlockContainerFactory>().To<BlockContainerFactory>().AsSingle()
                .WithArguments(blockContainerPrefab);

            Container.Bind<GameManagerHelpers>().AsSingle();

            Container.Bind<ILevelRepository>().FromInstance(levelRepository).AsSingle();


            var levelData = levelRepository.GetLevelData();
            Container.Bind<Board>().AsSingle().WithArguments(levelData.width, levelData.height, grid);
        }
    }
}