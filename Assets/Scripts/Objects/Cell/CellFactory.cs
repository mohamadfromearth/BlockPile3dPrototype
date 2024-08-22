using Event;
using UnityEngine;
using Zenject;

namespace Objects.Cell
{
    public class CellFactory : ICellFactory
    {
        private DefaultCell _defaultCellPrefab;

        [Inject] private EventChannel _channel;

        private Transform _parent;

        public CellFactory(DefaultCell defaultCellPrefab, Transform parent)
        {
            _defaultCellPrefab = defaultCellPrefab;
            _parent = parent;           
        }

        public ICell Create()
        {
            var cell = Object.Instantiate(_defaultCellPrefab, _parent);
            cell.Channel = _channel;
            return cell;
        }
    }
}