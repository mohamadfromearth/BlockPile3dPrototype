using System;
using System.Collections.Generic;
using Data;
using DG.Tweening;
using Objects.Block;
using Objects.BlocksContainer;
using UnityEngine;
using Utils;
using Zenject;
using Random = UnityEngine.Random;

[Serializable]
public struct BlockContainerSelectionBarData
{
    public List<Transform> containersPositionList;
    public List<Transform> containersSpawningPositionList;
    public Transform blockSelectionBarTransform;
    public float movingDuration;
    public int count;
    public Ease movingEase;
}

public class BlockContainerSelectionBar
{
    [Inject] private IBlockContainerFactory _blockContainerFactory;
    [Inject] private IBlockFactory _blockFactory;
    [Inject] private ColorRepository _colorRepository;


    private IBlockContainer[] _blockContainers = new IBlockContainer[3];

    private BlockContainerSelectionBarData _data;


    public int Count { get; private set; }

    public BlockContainerSelectionBar(BlockContainerSelectionBarData data)
    {
        _data = data;
    }


    public void Spawn(List<string> colors)
    {
        Spawn(colors, _data.blockSelectionBarTransform.position);
    }

    public void Spawn(List<string> colors, Vector3 parentPos)
    {
        _data.blockSelectionBarTransform.position = parentPos;
        Count = _data.count;


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

        for (int positionIndex = 0; positionIndex < _data.containersPositionList.Count; positionIndex++)
        {
            var position = _data.containersSpawningPositionList[positionIndex].position;
            var targetPos = _data.containersPositionList[positionIndex];

            var container = _blockContainerFactory.Create();

            _blockContainers[positionIndex] = container;

            container.SetPosition(position);
            container.MoveTo(targetPos.position, _data.movingDuration, _data.movingEase);

            var colorsCount = Random.Range(2, 4);


            for (int i = 0; i < colorsCount; i++)
            {
                var blockCount = Random.Range(1, 5);


                var colorIndex = Random.Range(0, colors.Count);

                for (int blockIndex = 0; blockIndex < blockCount; blockIndex++)
                {
                    var block = _blockFactory.Create(colors[colorIndex].ToColorIndex());


                    block.Color = _colorRepository.GetColor(colors[colorIndex]);


                    container.Push(block);
                }
            }


            container.SetCountText(container.Count.ToString(), 0);
        }
    }


    public void BackToInitialPosition(IBlockContainer blockContainer, float duration)
    {
        for (int i = 0; i < _blockContainers.Length; i++)
        {
            if (blockContainer == _blockContainers[i])
            {
                blockContainer.MoveTo(_data.containersPositionList[i].position, duration);
                break;
            }
        }
    }


    public void Clear()
    {
        for (int i = 0; i < _blockContainers.Length; i++)
        {
            var container = _blockContainers[i];
            if (container?.IsPlaced == false)
            {
                container?.Destroy(true);
                _blockContainers[i] = null;
            }
        }
    }

    public void Refresh(List<string> colors)
    {
        Clear();
        Spawn(colors, _data.blockSelectionBarTransform.position);
    }

    public void Decrease() => Count--;
}