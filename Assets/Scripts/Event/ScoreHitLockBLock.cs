using Objects.LockBlock;

namespace Event
{
    public struct ScoreHitLockBLock : IEventData
    {
        public ILockBlock LockBlock;

        public ScoreHitLockBLock(ILockBlock lockBlock)
        {
            LockBlock = lockBlock;
        }
    }
}