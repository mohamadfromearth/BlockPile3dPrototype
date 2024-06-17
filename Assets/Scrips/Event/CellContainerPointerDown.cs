using Event;
using Scrips.Objects.BlocksContainer;
using Scrips.Objects.CellsContainer;

namespace Scrips.Event
{
    public struct CellContainerPointerDown : IEventData
    {
        public IBlockContainer BlockContainer { get; }

        public CellContainerPointerDown(IBlockContainer blockContainer)
        {
            BlockContainer = blockContainer;
        }
    }
}