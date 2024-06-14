using Event;
using Scrips.Objects.CellsContainer;

namespace Scrips.Event
{
    public struct CellContainerPointerUp : IEventData
    {
        public ICellContainer CellContainer { get; }

        public CellContainerPointerUp(ICellContainer cellContainer)
        {
            CellContainer = cellContainer;
        }
    }
}