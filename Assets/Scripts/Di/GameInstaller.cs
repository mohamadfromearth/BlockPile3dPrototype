using System.Collections.Generic;
using System.Linq;
using Event;
using Objects.Block;
using Objects.BlocksContainer;
using Scrips;
using Scrips.Data;
using Scrips.Objects.BlockContainerHolder;
using Scrips.Objects.Cell;
using Scrips.Objects.CellsContainer;
using Scripts.Data;
using UnityEngine;
using Zenject;

namespace Di
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

        // selectionBar
        [SerializeField] private List<Color> colors;
        [SerializeField] private List<Transform> selectionBarPositionList;

        public override void InstallBindings()
        {
            Container.Bind<EventChannel>().AsSingle().NonLazy();

            Container.Bind<IBlockFactory>().To<BlockFactory>().AsSingle().WithArguments(blockPrefab);

            Container.Bind<IBlockContainerHolderFactory>().To<BlockContainerHolderFactory>().AsSingle()
                .WithArguments(blockContainerHolderPrefab).NonLazy();

            Container.Bind<IBlockContainerFactory>().To<BlockContainerFactory>().AsSingle()
                .WithArguments(blockContainerPrefab);


            Container.Bind<ILevelRepository>().FromInstance(levelRepository).AsSingle();


            var levelData = levelRepository.GetLevelData();
            Container.Bind<Board>().AsSingle().WithArguments(levelData.width, levelData.height, grid);

            Container.Bind<BlockContainerSelectionBar>().AsCached()
                .WithArguments(colors, selectionBarPositionList.Select(t => t.position).ToList());
        }
    }
}