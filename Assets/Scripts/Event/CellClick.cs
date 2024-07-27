using Objects.Cell;

namespace Event
{
    public struct CellClick : IEventData
    {
        public readonly ICell Cell;

        public CellClick(ICell cell)
        {
            Cell = cell;
        }
    }
}