namespace Event
{
    public struct BlockDestroy:IEventData
    {
        public int Count;

        public BlockDestroy(int count)
        {
            Count = count;
        }
    }
}