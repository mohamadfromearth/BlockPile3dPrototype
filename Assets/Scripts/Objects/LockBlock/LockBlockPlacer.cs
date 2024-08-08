using Data;
using Zenject;

namespace Objects.LockBlock
{
    public class LockBlockPlacer
    {
        [Inject] private ILockBlockFactory _lockBlockFactory;
        [Inject] private Board _board;
        [Inject] private ILevelRepository _levelRepository;

        public void Place()
        {
            var lockBlocksDataList = _levelRepository.GetLevelData().lockBlocks;

            foreach (var lockBlockData in lockBlocksDataList)
            {
                var worldPosition = _board.CellToWorld(lockBlockData.position);
                var lockBlock = _lockBlockFactory.Create();
                lockBlock.SetPosition(worldPosition);
                lockBlock.Count = lockBlockData.count;

                _board.GetCell(lockBlockData.position).CanPlaceItem = false;
                _board.AddLockBlock(lockBlock, lockBlockData.position);
            }
        }
    }
}