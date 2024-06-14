using UnityEngine;

namespace Scrips.Objects.CellContainerHolder
{
    public interface ICellContainerHolderFactory
    {
        public ICellContainerHolder Create();
    }
}