using Event;
using UnityEngine;
using Zenject;

namespace Objects.BlocksContainer
{
    public class BlockContainerFactory : IBlockContainerFactory
    {
        private BlockContainer _blockContainerPrefab;

        [Inject] private EventChannel _channel;

        public BlockContainerFactory(BlockContainer blockContainerPrefab)
        {
            this._blockContainerPrefab = blockContainerPrefab;
        }

        public IBlockContainer Create()
        {
            var cellContainer = Object.Instantiate(_blockContainerPrefab);
            cellContainer.Channel = _channel;
            return cellContainer;
        }
    }
}