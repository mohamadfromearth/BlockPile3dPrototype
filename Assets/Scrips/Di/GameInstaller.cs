using Event;
using Scrips.Objects.Cell;
using Scrips.Objects.CellContainerHolder;
using Scrips.Objects.CellsContainer;
using UnityEngine;
using Zenject;

namespace Scrips.Di
{
    public class GameInstaller : MonoInstaller
    {
        // prefabs
        [SerializeField] private Cell cellPrefab;
        [SerializeField] private CellContainer cellContainerPrefab;
        [SerializeField] private CellContainerHolder cellContainerHolderPrefab;


        public override void InstallBindings()
        {
            Container.Bind<EventChannel>().AsSingle().NonLazy();

            Container.Bind<ICellFactory>().To<CellFactory>().AsSingle().WithArguments(cellPrefab);

            Container.Bind<ICellContainerHolderFactory>().To<CellContainerHolderFactory>().AsSingle()
                .WithArguments(cellContainerPrefab);

            Container.Bind<ICellContainerFactory>().To<CellContainerFactory>().AsSingle()
                .WithArguments(cellContainerPrefab);

            Container.Bind<GameManagerHelpers>().AsSingle();
        }
    }
}