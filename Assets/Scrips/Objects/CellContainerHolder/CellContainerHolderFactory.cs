using UnityEngine;

namespace Scrips.Objects.CellContainerHolder
{
    public class CellContainerHolderFactory : ICellContainerHolderFactory
    {
        private CellContainerHolder _cellContainerHolderPrefab;


        public CellContainerHolderFactory(CellContainerHolder cellContainerHolderPrefab)
        {
            _cellContainerHolderPrefab = cellContainerHolderPrefab;
        }

        public ICellContainerHolder Create()
        {
            return Object.Instantiate(_cellContainerHolderPrefab);
        }
    }
}