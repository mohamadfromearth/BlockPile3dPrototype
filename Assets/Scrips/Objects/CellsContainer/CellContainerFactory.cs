using Event;
using UnityEngine;
using Zenject;

namespace Scrips.Objects.CellsContainer
{
    public class CellContainerFactory : ICellContainerFactory
    {
        private CellContainer cellContainerPrefab;

        [Inject] private EventChannel _channel;

        public CellContainerFactory(CellContainer cellContainerPrefab)
        {
            this.cellContainerPrefab = cellContainerPrefab;
        }

        public ICellContainer Create()
        {
            var cellContainer = Object.Instantiate(cellContainerPrefab);
            cellContainer.Channel = _channel;
            return cellContainer;
        }
    }
}