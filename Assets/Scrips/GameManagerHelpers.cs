using System.Collections.Generic;
using Scrips.Data;
using Scrips.Objects.Cell;
using Scrips.Objects.CellsContainer;
using UnityEngine;
using Zenject;

namespace Scrips
{
    public class GameManagerHelpers
    {
        [Inject] private IBlockContainerFactory _blockContainerFactory;
        [Inject] private IBlockFactory _blockFactory;
        [Inject] private ILevelRepository _levelRepository;
        [Inject] private Board _board;


        public void SpawnSelectionBarBlockContainers(List<Vector3> blockContainersPositionList)
        {
            var selectionBarBlockContainersData = _levelRepository.GetLevelData().selectionBarBlockContainerDataList;


            for (int i = 0; i < selectionBarBlockContainersData.Count; i++)
            {
                var containerData = selectionBarBlockContainersData[i];
                var container = _blockContainerFactory.Create();
                container.SetPosition(blockContainersPositionList[i]);

                for (int height = 0; height < containerData.count; height++)
                {
                    var block = _blockFactory.Create();
                    var blockPosition = container.GetPosition();
                    blockPosition.y = height * 0.2f;
                    block.SetPosition(blockPosition);
                    block.Color = containerData.color;
                    block.GameObj.transform.SetParent(container.GameObj.transform);

                    container.Push(block);
                }
            }
        }

        public void SpawnBoardBlockContainers()
        {
            var levelData = _levelRepository.GetLevelData();

            var blockContainerList = levelData.blockContainerDataList;

            foreach (var blockContainerData in blockContainerList)
            {
                var blockContainer = _blockContainerFactory.Create();
                blockContainer.SetPosition(blockContainerData.position);

                for (int i = 0; i < blockContainerData.count; i++)
                {
                    var block = _blockFactory.Create();
                    var blockPosition = blockContainer.GetPosition();
                    blockPosition.y = i * 0.2f;
                    block.SetPosition(blockPosition);
                    block.GameObj.transform.SetParent(blockContainer.GameObj.transform);
                    block.Color = blockContainerData.color;

                    blockContainer.Push(block);
                }

                _board.AddBlockContainer(blockContainer, blockContainerData.position);
            }
        }
    }
}