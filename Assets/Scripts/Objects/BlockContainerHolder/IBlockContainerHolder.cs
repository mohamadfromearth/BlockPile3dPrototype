using Core;
using Objects.BlocksContainer;

namespace Objects.BlockContainerHolder
{
    public interface IBlockContainerHolder : IPosition, IGameObject, IToScale
    {
        public IBlockContainer BlockContainer { get; set; }
    }
}