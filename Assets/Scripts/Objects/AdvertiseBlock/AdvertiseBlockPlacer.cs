using Data;
using Zenject;

namespace Objects.AdvertiseBlock
{
    public class AdvertiseBlockPlacer
    {
        [Inject] private IAdvertiseBlockFactory _advertiseBlockFactory;
        [Inject] private Board _board;
        [Inject] private ILevelRepository _levelRepository;


        public void Place()
        {
            var advertiseBlocksPositions = _levelRepository.GetLevelData().advertiseBlocks;

            foreach (var position in advertiseBlocksPositions)
            {
                var worldPosition = _board.CellToWorld(position);
                var advertiseBlock = _advertiseBlockFactory.Create();
                advertiseBlock.SetPosition(worldPosition);

                _board.GetCell(position).CanPlaceItem = false;
                _board.AddAdvertiseBlock(advertiseBlock, position);
            }
        }
    }
}