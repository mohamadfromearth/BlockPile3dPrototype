using System;
using Object = UnityEngine.Object;

namespace Scrips.Objects.Cell
{
    public class CellFactory : ICellFactory
    {
        private Cell _cellPrefab;


        public CellFactory(Cell cellPrefab)
        {
            _cellPrefab = cellPrefab;
        }


        public ICell Create() => Object.Instantiate(_cellPrefab);
    }
}