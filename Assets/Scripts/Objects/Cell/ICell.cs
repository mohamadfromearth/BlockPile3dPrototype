using Core;
using Objects.BlocksContainer;
using UnityEngine;

namespace Objects.Cell
{
    public interface ICell : IPosition, IGameObject, IToScale, IColor
    {
        public bool CanPlaceItem { get; set; }
        public IBlockContainer BlockContainer { get; set; }


        public void Destroy();
    }
}