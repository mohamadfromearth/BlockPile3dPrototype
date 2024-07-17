using Objects.BlockContainerHolder;
using UnityEngine;

namespace Scrips.Objects.BlockContainerHolder
{
    public class BlockContainerHolderFactory : IBlockContainerHolderFactory
    {
        private global::Objects.BlockContainerHolder.BlockContainerHolder _blockContainerHolderPrefab;


        public BlockContainerHolderFactory(global::Objects.BlockContainerHolder.BlockContainerHolder blockContainerHolderPrefab)
        {
            _blockContainerHolderPrefab = blockContainerHolderPrefab;
        }

        public IBlockContainerHolder Create()
        {
            return Object.Instantiate(_blockContainerHolderPrefab);
        }
    }
}