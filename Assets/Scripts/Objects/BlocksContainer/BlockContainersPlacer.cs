using Data;
using Objects.Block;
using UnityEngine;
using Utils;
using Zenject;

namespace Objects.BlocksContainer
{
    public class BlockContainersPlacer
    {
        [Inject] private ILevelRepository _levelRepository;
        [Inject] private Board _board;
        [Inject] private IBlockContainerFactory _blockContainerFactory;
        [Inject] private IBlockFactory _blockFactory;
        [Inject] private ColorRepository _colorRepository;


        public void Place()
        {
            var levelData = _levelRepository.GetLevelData();
            var containerDataList = levelData.blockContainerDataList;

            foreach (var containerData in containerDataList)
            {
                var cell = _board.GetCell(containerData.position);

                var container = _blockContainerFactory.Create();
                container.IsPlaced = true;

                _board.AddBlockContainer(container, containerData.position);
                container.SetParent(cell.GameObj.transform.parent);

                foreach (var color in containerData.color)
                {
                    var block = _blockFactory.Create(color.ToColorIndex());
                    block.Color = _colorRepository.GetColor(color);

                    container.Push(block);
                }

                container.SetCountText(container.Count.ToString(), 0);
            }
        }
    }
}