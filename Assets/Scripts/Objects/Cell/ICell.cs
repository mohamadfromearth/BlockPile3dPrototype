using Core;
using Objects.BlocksContainer;

namespace Objects.Cell
{
    public interface ICell : IPosition, IGameObject, IToScale
    {
        public IBlockContainer BlockContainer { get; set; }
    }
}