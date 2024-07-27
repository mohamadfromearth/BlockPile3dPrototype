using System.ComponentModel;
using Event;
using Objects.Block;
using Objects.BlocksContainer;
using Objects.Cell;
using Scrips.Objects.Cell;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Designer.DI
{
    public class DesignerInstaller : MonoInstaller
    {
        // prefabs
        [SerializeField] private Block blockPrefab;
        [SerializeField] private BlockContainer blockContainerPrefab;

        [FormerlySerializedAs("cellPrefab")] [FormerlySerializedAs("blockContainerHolderPrefab")] [SerializeField]
        private DefaultCell defaultCellPrefab;

        [SerializeField] private Grid grid;

        [SerializeField] private int width, height;

        public override void InstallBindings()
        {
            Container.Bind<EventChannel>().AsSingle();


            Container.Bind<IBlockFactory>().To<BlockFactory>().AsSingle().WithArguments(blockPrefab);

            Container.Bind<ICellFactory>().To<CellFactory>().AsSingle()
                .WithArguments(defaultCellPrefab).NonLazy();

            Container.Bind<Board>().AsSingle().WithArguments(width, height, grid);
        }
    }
}