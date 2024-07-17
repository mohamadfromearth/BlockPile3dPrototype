using System.Collections.Generic;
using Objects.BlockContainerHolder;
using Objects.BlocksContainer;
using Scrips.Objects.BlockContainerHolder;
using UnityEngine;
using Zenject;

public class Board
{
    private readonly Grid _grid;

    private readonly int _width;
    private readonly int _height;

    private IBlockContainerHolderFactory _blockContainerHolderFactory;


    private Dictionary<Vector3Int, IBlockContainerHolder> _blockContainerHoldersDic = new();

    public Board(int width, int height, Grid grid)
    {
        _width = width;
        _height = height;
        _grid = grid;
    }

    [Inject]
    private void Construct(IBlockContainerHolderFactory blockContainerHolderFactory)
    {
        _blockContainerHolderFactory = blockContainerHolderFactory;
    }


    public IBlockContainerHolder GetBlockContainerHolder(Vector3 worldPosition)
    {
        var gridPos = _grid.WorldToCell(worldPosition);
        if (_blockContainerHoldersDic.TryGetValue(gridPos, out var holder))
        {
            return holder;
        }

        return null;
    }


    public IBlockContainerHolder GetBlockContainerHolder(Vector3Int boardPosition)
    {
        if (_blockContainerHoldersDic.TryGetValue(boardPosition, out var holder))
        {
            return holder;
        }

        return null;
    }

    public void AddBlockContainer(IBlockContainer blockContainer, Vector3 worldPosition)
    {
        var gridPos = _grid.WorldToCell(worldPosition);

        if (_blockContainerHoldersDic.TryGetValue(gridPos, out var holder))
        {
            holder.BlockContainer = blockContainer;
        }
    }

    public void AddBlockContainer(IBlockContainer blockContainer, Vector3Int gridPosition)
    {
        if (_blockContainerHoldersDic.TryGetValue(gridPosition, out var holder))
        {
            holder.BlockContainer = blockContainer;
            blockContainer.SetPosition(holder.GetPosition());
        }
    }


    public Vector3Int WorldToCell(Vector3 worldPosition) => _grid.WorldToCell(worldPosition);


    public void Clear()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                var position = new Vector3Int(x, 0, z);
                var holder = GetBlockContainerHolder(position);

                if (holder != null && holder.BlockContainer != null)
                {
                    holder.BlockContainer.Destroy(true);
                    holder.BlockContainer = null;
                }
            }
        }
    }


    public void SpawnHolders(List<Vector3Int> emptyHolders)
    {
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                var gridPos = new Vector3Int(x, 0, y);

                if (emptyHolders.Contains(gridPos)) continue;

                var pos = _grid.CellToWorld(gridPos);

                var holder = _blockContainerHolderFactory.Create();
                holder.SetPosition(pos);

                _blockContainerHoldersDic[gridPos] = holder;
            }
        }
    }
}