using Event;
using UnityEngine;
using Zenject;

namespace Objects.LockBlock
{
    public class LockBlockFactory : ILockBlockFactory
    {
        private readonly LockBlock _lockBlockPrefab;
        [Inject] private EventChannel _channel;


        public LockBlockFactory(LockBlock lockBlockPrefab)
        {
            _lockBlockPrefab = lockBlockPrefab;
        }


        public ILockBlock Create()
        {
            var lockBlock = Object.Instantiate(_lockBlockPrefab);
            lockBlock.Channel = _channel;
            return lockBlock;
        }
    }
}