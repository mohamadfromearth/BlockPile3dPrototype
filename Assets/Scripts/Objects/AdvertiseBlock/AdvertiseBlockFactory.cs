using UnityEngine;

namespace Objects.AdvertiseBlock
{
    public class AdvertiseBlockFactory : IAdvertiseBlockFactory
    {
        private readonly AdvertiseBlock _advertiseBlockPrefab;

        public AdvertiseBlockFactory(AdvertiseBlock advertiseBlockPrefab)
        {
            _advertiseBlockPrefab = advertiseBlockPrefab;
        }

        public IAdvertiseBlock Create() => Object.Instantiate(_advertiseBlockPrefab);
    }
}