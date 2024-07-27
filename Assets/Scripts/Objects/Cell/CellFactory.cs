using Event;
using UnityEngine;
using Zenject;

namespace Objects.Cell
{
    public class CellFactory : ICellFactory
    {
        private DefaultCell _defaultCellPrefab;

        [Inject] private EventChannel _channel;

        public CellFactory(DefaultCell defaultCellPrefab)
        {
            _defaultCellPrefab = defaultCellPrefab;
        }

        public ICell Create()
        {
            var cell = Object.Instantiate(_defaultCellPrefab);
            cell.Channel = _channel;
            return cell;
        }
    }
}