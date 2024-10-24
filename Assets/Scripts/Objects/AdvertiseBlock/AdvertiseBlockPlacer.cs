﻿using Data;
using Utils;
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
                worldPosition.y += 0.1f;
                var cell = _board.GetCell(position);
                var advertiseBlock = _advertiseBlockFactory.Create();
                advertiseBlock.SetPosition(worldPosition);
                advertiseBlock.GameObj.transform.SetParent(cell.GameObj.transform.parent);
                _board.AddAdvertiseBlock(advertiseBlock, position);
            }
        }
    }
}