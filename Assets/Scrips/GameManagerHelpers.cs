using System.Collections.Generic;
using Scrips.Objects.Cell;
using Scrips.Objects.CellsContainer;
using UnityEngine;
using Zenject;

namespace Scrips
{
    public class GameManagerHelpers
    {
        [Inject] private ICellContainerFactory _cellContainerFactory;
        [Inject] private ICellFactory _cellFactory;


        public void SpawnCellContainers(List<Vector3> selectionBarCellContainerPosList)
        {
            foreach (var position in selectionBarCellContainerPosList)
            {
                var cellContainer = _cellContainerFactory.Create();
                cellContainer.SetPosition(position);

                for (int i = 0; i < 3; i++)
                {
                    var cell = _cellFactory.Create();
                    var cellPosition = cellContainer.GetPosition();
                    cellPosition.y = i * 0.2f;
                    cell.SetPosition(cellPosition);
                    cell.GameObj.transform.SetParent(cellContainer.GameObj.transform);

                    cellContainer.Push(cell);
                }
            }
        }
    }
}