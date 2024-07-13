using Objects.BlocksContainer;

namespace Event
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