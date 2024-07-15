using Core;
using Objects.BlocksContainer;
using Scrips.Objects.CellsContainer;

namespace Scrips.Objects.BlockContainerHolder
{
    public interface IBlockContainerHolder : IPosition, IGameObject, IToScale
    {
        public IBlockContainer BlockContainer { get; set; }
    }
}