using System.Collections.Generic;
using Objects.BlocksContainer;
using Scrips.Objects.Cell;
using Scrips.Objects.CellsContainer;
using UnityEngine;
using Zenject;

public class BlockContainerSelectionBar
{
    private readonly List<Color> _colors;
    [Inject] private IBlockContainerFactory _blockContainerFactory;
    [Inject] private IBlockFactory _blockFactory;
    private readonly List<Vector3> _containersPositionList;


    public int Count { get; private set; }

    public BlockContainerSelectionBar(List<Color> colors,
        List<Vector3> containersPositionList)
    {
        _colors = colors;
        _containersPositionList = containersPositionList;
        Count = 3;
    }


    public void Spawn()
    {
        Count = _containersPositionList.Count;
        foreach (var position in _containersPositionList)
        {
            var container = _blockContainerFactory.Create();
            container.SetPosition(position);


            var colorsCount = Random.Range(1, 4);

            for (int i = 0; i < colorsCount; i++)
            {
                var blockCount = Random.Range(1, 8);

                var colorIndex = Random.Range(0, _colors.Count);


                for (int blockIndex = 0; blockIndex < blockCount; blockIndex++)
                {
                    var block = _blockFactory.Create();

                    block.Color = _colors[colorIndex];

                    container.Push(block);
                }
            }
        }
    }

    public void Decrease() => Count--;
}