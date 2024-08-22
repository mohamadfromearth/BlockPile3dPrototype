using Core;
using Objects.AdvertiseBlock;
using Objects.BlocksContainer;
using Objects.LockBlock;
using UnityEngine;

namespace Objects.Cell
{
    public interface ICell : IPosition, IGameObject, IToScale, IColor
    {
        public bool CanPlaceItem { get; set; }
        public IBlockContainer BlockContainer { get; set; }
        public IAdvertiseBlock AdvertiseBlock { get; set; }
        public ILockBlock LockBlock { get; set; }
        public void Destroy();
    }
}