using Event;
using Objects.AdvertiseBlock;
using Objects.Block;
using Objects.BlocksContainer;
using Objects.Cell;
using Objects.LockBlock;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Designer.DI
{
    public class DesignerInstaller : MonoInstaller
    {
        // prefabs
        [SerializeField] private BlockContainer blockContainerPrefab;
        [SerializeField] private AdvertiseBlock advertiseBlockPrefab;
        [SerializeField] private LockBlock lockBlockPrefab;
        [SerializeField] private Block[] blockPrefabs;


        [FormerlySerializedAs("cellPrefab")] [FormerlySerializedAs("blockContainerHolderPrefab")] [SerializeField]
        private DefaultCell defaultCellPrefab;

        [SerializeField] private Grid grid;

        [SerializeField] private int width, height;

        public override void InstallBindings()
        {
            Container.Bind<EventChannel>().AsSingle();


            Container.Bind<IBlockFactory>().To<BlockFactory>().AsSingle().WithArguments(blockPrefabs);

            Container.Bind<ICellFactory>().To<CellFactory>().AsSingle()
                .WithArguments(defaultCellPrefab, grid.transform).NonLazy();

            Container.Bind<Board>().AsSingle().WithArguments(width, height, grid, grid.transform);

            Container.Bind<IBlockContainerFactory>().To<BlockContainerFactory>().AsSingle()
                .WithArguments(blockContainerPrefab);

            Container.Bind<ILockBlockFactory>().To<LockBlockFactory>().AsSingle()
                .WithArguments(lockBlockPrefab);


            Container.Bind<IAdvertiseBlockFactory>().To<AdvertiseBlockFactory>().AsSingle()
                .WithArguments(advertiseBlockPrefab);
        }
    }
}