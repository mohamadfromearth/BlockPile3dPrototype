using Core;

namespace Objects.LockBlock
{
    public interface ILockBlock : IPosition
    {
        public int Count { get; set; }

        public void Destroy();
    }
}