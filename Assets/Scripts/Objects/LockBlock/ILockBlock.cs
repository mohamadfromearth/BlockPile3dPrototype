using Core;

namespace Objects.LockBlock
{
    public interface ILockBlock : IPosition, IGameObject
    {
        public int Count { get; set; }

        public void Destroy();
    }
}