using Scrips.Core;
using Scrips.Objects.CellsContainer;

namespace Scrips.Objects.CellContainerHolder
{
    public interface ICellContainerHolder : IPosition
    {
        public ICellContainer CellContainer { get; set; }
    }
}