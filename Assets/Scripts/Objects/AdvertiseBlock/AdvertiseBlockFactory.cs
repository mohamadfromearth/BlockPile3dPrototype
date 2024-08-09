using Event;
using UnityEngine;
using Zenject;

namespace Objects.AdvertiseBlock
{
    public class AdvertiseBlockFactory : IAdvertiseBlockFactory
    {
        private readonly AdvertiseBlock _advertiseBlockPrefab;
        [Inject] private EventChannel _channel;

        public AdvertiseBlockFactory(AdvertiseBlock advertiseBlockPrefab)
        {
            _advertiseBlockPrefab = advertiseBlockPrefab;
        }

        public IAdvertiseBlock Create()
        {
            var advertiseBlock = Object.Instantiate(_advertiseBlockPrefab);
            advertiseBlock.Channel = _channel;
            return advertiseBlock;
        }
    }
}