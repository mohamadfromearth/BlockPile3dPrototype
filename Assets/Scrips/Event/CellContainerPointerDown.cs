using Event;
using Scrips.Objects.CellsContainer;

namespace Scrips.Event
{
    public struct CellContainerPointerDown : IEventData
    {
        public ICellContainer CellContainer { get; }

        public CellContainerPointerDown(ICellContainer cellContainer)
        {
            CellContainer = cellContainer;
        }
    }
}