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


                var previousCount = 0;


                for (int blockGroupIndex = 0;
                     blockGroupIndex < containerData.blockGroupDataList.Count;
                     blockGroupIndex++)
                {
                    var blockGroup = containerData.blockGroupDataList[blockGroupIndex];

                    for (int blockGroupHeight = previousCount;
                         blockGroupHeight < previousCount + blockGroup.count;
                         blockGroupHeight++)
                    {
                        var block = _blockFactory.Create();
                        var blockPosition = container.GetPosition();
                        blockPosition.y = blockGroupHeight * 0.2f;
                        block.SetPosition(blockPosition);
                        block.Color = blockGroup.color;
                        block.GameObj.transform.SetParent(container.GameObj.transform);

                        container.Push(block);
                    }

                    previousCount += blockGroup.count;
                }
            }
        }

        public void SpawnBoardBlockContainers()
        {
            var levelData = _levelRepository.GetLevelData();

            var containerDataList = levelData.blockContainerDataList;


            foreach (var containerData in containerDataList)
            {
                var blockContainer = _blockContainerFactory.Create();
                blockContainer.SetPosition(containerData.position);

                var previousCount = 0;

                for (int blockGroupIndex = 0;
                     blockGroupIndex < containerData.blockGroupDataList.Count;
                     blockGroupIndex++)
                {
                    var blockGroup = containerData.blockGroupDataList[blockGroupIndex];

                    for (int blockGroupHeight = previousCount;
                         blockGroupHeight < previousCount + blockGroup.count;
                         blockGroupHeight++)
                    {
                        var block = _blockFactory.Create();
                        var blockPosition = blockContainer.GetPosition();
                        blockPosition.y = blockGroupHeight * 0.2f + 0.2f;
                        block.SetPosition(blockPosition);
                        block.Color = blockGroup.color;
                        block.GameObj.transform.SetParent(blockContainer.GameObj.transform);

                        blockContainer.Push(block);
                    }

                    previousCount += blockGroup.count;
                }


                _board.AddBlockContainer(blockContainer, containerData.position);
            }
        }
    }
}