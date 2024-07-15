using System.ComponentModel;
using Objects.Block;
using Objects.BlocksContainer;
using Scrips.Objects.BlockContainerHolder;
using Scrips.Objects.Cell;
using UnityEngine;
using Zenject;

namespace Designer.DI
{
    public class DesignerInstaller : MonoInstaller
    {
        // prefabs
        [SerializeField] private Block blockPrefab;
        [SerializeField] private BlockContainer blockContainerPrefab;
        [SerializeField] private BlockContainerHolder blockContainerHolderPrefab;

        [SerializeField] private Grid grid;

        [SerializeField] private int width, height;

        public override void InstallBindings()
        {
            Container.Bind<IBlockFactory>().To<BlockFactory>().AsSingle().WithArguments(blockPrefab);

            Container.Bind<IBlockContainerHolderFactory>().To<BlockContainerHolderFactory>().AsSingle()
                .WithArguments(blockContainerHolderPrefab).NonLazy();

            Container.Bind<Board>().AsSingle().WithArguments(width, height, grid);
        }
    }
}