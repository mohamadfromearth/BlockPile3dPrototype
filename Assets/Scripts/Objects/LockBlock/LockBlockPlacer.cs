using Data;
using Utils;
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
                var cell = _board.GetCell(lockBlockData.position);
                var lockBlock = _lockBlockFactory.Create();
                lockBlock.SetPosition(cell.GetPosition());
                lockBlock.GameObj.transform.SetParent(cell.GameObj.transform.parent);
                lockBlock.Count = lockBlockData.count;
                cell.LockBlock = lockBlock;
                cell.CanPlaceItem = false;
                _board.AddLockBlock(lockBlock, lockBlockData.position);
            }
        }
    }
}