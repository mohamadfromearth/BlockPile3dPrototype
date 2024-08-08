using System.Collections.Generic;
using Data;
using Objects.Block;
using Objects.BlocksContainer;
using UnityEngine;
using Zenject;

public class BlockContainerSelectionBar
{
    [Inject] private IBlockContainerFactory _blockContainerFactory;
    [Inject] private IBlockFactory _blockFactory;
    [Inject] private ColorRepository _colorRepository;
    private readonly List<Vector3> _containersPositionList;


    public int Count { get; private set; }

    public BlockContainerSelectionBar(List<Vector3> containersPositionList)
    {
        _containersPositionList = containersPositionList;
        Count = 3;
    }


    public void Spawn(List<string> colors)
    {
        Count = _containersPositionList.Count;

        // foreach (var position in _containersPositionList)
        // {
        //     var container = _blockContainerFactory.Create();
        //     container.SetPosition(position);
        //
        //
        //     var colorsCount = 2;
        //
        //     var isRedInt = Random.Range(0, 2);
        //     bool isRed = isRedInt != 0;
        //
        //     for (int i = 0; i < colorsCount; i++)
        //     {
        //         var blockCount = Random.Range(1, 8);
        //
        //         var colorIndex = Random.Range(0, _colors.Count - 1);
        //
        //
        //         for (int blockIndex = 0; blockIndex < blockCount; blockIndex++)
        //         {
        //             var block = _blockFactory.Create();
        //
        //
        //             if (i == 0)
        //             {
        //                 block.Color = isRed ? Color.black : Color.red;
        //             }
        //             else
        //             {
        //                 block.Color = isRed ? Color.red : Color.black;
        //             }
        //
        //             container.Push(block);
        //         }
        //     }
        // }  

        foreach (var position in _containersPositionList)
        {
            var container = _blockContainerFactory.Create();
            container.SetPosition(position);


            var colorsCount = Random.Range(2, 4);

            for (int i = 0; i < colorsCount; i++)
            {
                var blockCount = Random.Range(1, 8);

                var colorIndex = Random.Range(0, colors.Count);


                for (int blockIndex = 0; blockIndex < blockCount; blockIndex++)
                {
                    var block = _blockFactory.Create();

                    block.Color = _colorRepository.GetColor(colors[colorIndex]);

                    container.Push(block);
                }
            }
        }
    }

    public void Decrease() => Count--;
}