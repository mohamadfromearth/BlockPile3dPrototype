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
                var pos = cell.GetPosition();
                pos.y += 0.1f;
                lockBlock.SetPosition(pos);
                lockBlock.GameObj.transform.SetParent(cell.GameObj.transform.parent);
                lockBlock.Count = lockBlockData.count;
                _board.AddLockBlock(lockBlock, lockBlockData.position);
            }
        }
    }
}