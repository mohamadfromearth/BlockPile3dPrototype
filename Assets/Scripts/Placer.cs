using Objects.AdvertiseBlock;
using Objects.BlocksContainer;
using Objects.LockBlock;
using Zenject;

public class Placer
{
    [Inject] private AdvertiseBlockPlacer _advertiseBlockPlacer;
    [Inject] private BlockContainersPlacer _blockContainersPlacer;
    [Inject] private LockBlockPlacer _lockBlockPlacer;


    public void Place()
    {
        _advertiseBlockPlacer.Place();
        _blockContainersPlacer.Place();
        _lockBlockPlacer.Place();
    }
}