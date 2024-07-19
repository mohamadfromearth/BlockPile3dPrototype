using Objects.Cell;

namespace Event
{
    public struct BlockContainerHolderClick : IEventData
    {
        public ICell Cell;

        public BlockContainerHolderClick(ICell cell)
        {
            Cell = cell;
        }
    }
}