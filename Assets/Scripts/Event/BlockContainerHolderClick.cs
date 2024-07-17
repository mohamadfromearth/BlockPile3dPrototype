using Objects.BlockContainerHolder;

namespace Event
{
    public struct BlockContainerHolderClick : IEventData
    {
        public IBlockContainerHolder BlockContainerHolder;

        public BlockContainerHolderClick(IBlockContainerHolder blockContainerHolder)
        {
            BlockContainerHolder = blockContainerHolder;
        }
    }
}