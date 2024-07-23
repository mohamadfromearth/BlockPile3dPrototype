using Core;
using Objects.BlocksContainer;

namespace Objects.Cell
{
    public interface ICell : IPosition, IGameObject, IToScale
    {
        public bool CanPlaceItem { get; set; }
        public IBlockContainer BlockContainer { get; set; }

        public void Destroy();
    }
}