using UnityEngine;

namespace Objects.Cell
{
    public class CellFactory : ICellFactory
    {
        private global::Objects.Cell.DefaultCell _defaultCellPrefab;


        public CellFactory(global::Objects.Cell.DefaultCell defaultCellPrefab)
        {
            _defaultCellPrefab = defaultCellPrefab;
        }

        public ICell Create()
        {
            return Object.Instantiate(_defaultCellPrefab);
        }
    }
}