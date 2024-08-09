using Objects.AdvertiseBlock;

namespace Event
{
    public struct AdvertiseBlockPointerDown : IEventData
    {
        public IAdvertiseBlock AdvertiseBlock;

        public AdvertiseBlockPointerDown(IAdvertiseBlock advertiseBlock)
        {
            AdvertiseBlock = advertiseBlock;
        }
    }
}