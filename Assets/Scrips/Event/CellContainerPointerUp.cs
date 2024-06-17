using Event;
using Scrips.Objects.CellsContainer;

namespace Scrips.Event
{
    public struct CellContainerPointerUp : IEventData
    {
        public IBlockContainer BlockContainer { get; }

        public CellContainerPointerUp(IBlockContainer blockContainer)
        {
            BlockContainer = blockContainer;
        }
    }
}