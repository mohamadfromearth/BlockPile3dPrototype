using UnityEngine;

namespace Objects.LockBlock
{
    public class LockBlockFactory : ILockBlockFactory
    {
        private readonly LockBlock _lockBlockPrefab;


        public LockBlockFactory(LockBlock lockBlockPrefab)
        {
            _lockBlockPrefab = lockBlockPrefab;
        }


        public ILockBlock Create() => Object.Instantiate(_lockBlockPrefab);
    }
}