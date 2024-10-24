using UnityEngine;

namespace Objects.NoneValueLockBlock
{
    public class NoneValueLockBlockFactory : INoneValueLockBlockFactory
    {
        private readonly NoneValueLockBlock _prefab;

        public NoneValueLockBlockFactory(NoneValueLockBlock prefab)
        {
            _prefab = prefab;
        }


        public INoneValueLockBlock Create() => Object.Instantiate(_prefab);
    }
}