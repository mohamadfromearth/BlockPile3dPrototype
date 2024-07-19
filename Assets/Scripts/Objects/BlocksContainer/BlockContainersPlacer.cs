using Scrips.Objects.Cell;
using Scrips.Objects.CellsContainer;
using Scripts.Data;
using Zenject;

public class BlockContainersPlacer
{
    [Inject] private ILevelRepository _levelRepository;
    [Inject] private Board _board;
    [Inject] private IBlockContainerFactory _blockContainerFactory;
    [Inject] private IBlockFactory _blockFactory;


    public void Place()
    {
        var levelData = _levelRepository.GetLevelData();
        var containerDataList = levelData.blockContainerDataList;

        foreach (var containerData in containerDataList)
        {
            var holder = _board.GetBlockContainerHolder(containerData.position);

            var container = _blockContainerFactory.Create();
            container.IsPlaced = true;

            holder.BlockContainer = container;

            container.SetPosition(holder.GetPosition());

            foreach (var color in containerData.color)
            {
                var block = _blockFactory.Create();
                block.Color = color;

                container.Push(block);
            }
        }
    }
}