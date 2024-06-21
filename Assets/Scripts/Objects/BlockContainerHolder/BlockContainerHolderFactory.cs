using UnityEngine;

namespace Scrips.Objects.BlockContainerHolder
{
    public class BlockContainerHolderFactory : IBlockContainerHolderFactory
    {
        private BlockContainerHolder _blockContainerHolderPrefab;


        public BlockContainerHolderFactory(BlockContainerHolder blockContainerHolderPrefab)
        {
            _blockContainerHolderPrefab = blockContainerHolderPrefab;
        }

        public IBlockContainerHolder Create()
        {
            return Object.Instantiate(_blockContainerHolderPrefab);
        }
    }
}