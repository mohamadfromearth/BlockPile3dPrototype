using System.Collections.Generic;
using Objects.AdvertiseBlock;
using Objects.BlocksContainer;
using Objects.Cell;
using Objects.LockBlock;
using UnityEngine;
using Zenject;


public class Board
{
    private readonly Grid _grid;

    public int Width { get; private set; }
    public int Height { get; private set; }


    private ICellFactory _cellFactory;


    private Dictionary<Vector3Int, ICell> _cellsDic = new();

    private int _cellItemsCount;

    public bool IsFilled => _cellItemsCount >= _cellsDic.Count;

    public Board(int width, int height, Grid grid)
    {
        _grid = grid;
    }

    [Inject]
    private void Construct(ICellFactory cellFactory)
    {
        _cellFactory = cellFactory;
    }


    public ICell GetCell(Vector3 worldPosition)
    {
        var gridPos = _grid.WorldToCell(worldPosition);
        if (_cellsDic.TryGetValue(gridPos, out var holder))
        {
            return holder;
        }

        return null;
    }


    public ICell GetCell(Vector3Int boardPosition)
    {
        if (_cellsDic.TryGetValue(boardPosition, out var holder))
        {
            return holder;
        }

        return null;
    }

    public Vector3Int GetCellPosition(ICell cell)
    {
        foreach (var keyValuePair in _cellsDic)
        {
            if (keyValuePair.Value.Equals(cell))
            {
                return keyValuePair.Key;
            }
        }

        return Vector3Int.zero;
    }

    public void AddBlockContainer(IBlockContainer blockContainer, Vector3 worldPosition)
    {
        var gridPos = _grid.WorldToCell(worldPosition);

        if (_cellsDic.TryGetValue(gridPos, out var cell))
        {
            _cellItemsCount += BoardHelpers.GetItemsCountModifier(blockContainer, cell);
            Debug.Log("CellItemCount is :" + _cellItemsCount);

            cell.CanPlaceItem = blockContainer == null;
            cell.BlockContainer = blockContainer;
        }
    }


    public void AddBlockContainer(IBlockContainer blockContainer, Vector3Int gridPosition)
    {
        if (_cellsDic.TryGetValue(gridPosition, out var cell))
        {
            cell.BlockContainer = blockContainer;
            blockContainer.SetPosition(cell.GetPosition());
        }
    }

    public void AddAdvertiseBlock(IAdvertiseBlock advertiseBlock, Vector3Int gridPosition)
    {
        if (_cellsDic.TryGetValue(gridPosition, out var cell))
        {
            _cellItemsCount += BoardHelpers.GetItemsCountModifier(advertiseBlock, cell);
            Debug.Log("CellItemCount is :" + _cellItemsCount);

            cell.CanPlaceItem = advertiseBlock == null;

            cell.AdvertiseBlock = advertiseBlock;
        }
    }

    public void AddAdvertiseBlock(IAdvertiseBlock advertiseBlock, Vector3 position)
    {
        var gridPos = WorldToCell(position);
        AddAdvertiseBlock(advertiseBlock, gridPos);
    }


    public void AddLockBlock(ILockBlock lockBlock, Vector3 position)
    {
        var gridPos = WorldToCell(position);
        AddLockBlock(lockBlock, gridPos);
    }

    public void AddLockBlock(ILockBlock lockBlock, Vector3Int gridPosition)
    {
        if (_cellsDic.TryGetValue(gridPosition, out var cell))
        {
            _cellItemsCount += BoardHelpers.GetItemsCountModifier(lockBlock, cell);
            Debug.Log("CellItemCount is :" + _cellItemsCount);

            cell.LockBlock = lockBlock;
            cell.CanPlaceItem = lockBlock == null;
        }
    }


    public Vector3Int WorldToCell(Vector3 worldPosition) => _grid.WorldToCell(worldPosition);

    public Vector3 CellToWorld(Vector3Int cellPosition) => _grid.CellToWorld(cellPosition);


    public void Clear()
    {
        _cellItemsCount = 0;

        for (int x = 0; x < Width; x++)
        {
            for (int z = 0; z < Height; z++)
            {
                var position = new Vector3Int(x, 0, z);
                var cell = GetCell(position);

                if (cell != null)
                {
                    cell.BlockContainer?.Destroy(true);
                    cell.AdvertiseBlock?.Destroy();
                    cell.LockBlock?.Destroy();
                    cell.Destroy();
                }
            }
        }

        _cellsDic.Clear();
    }


    public Dictionary<Vector3Int, ICell> SpawnCells(List<Vector3Int> emptyHolders, int width, int height)
    {
        Width = width;
        Height = height;


        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var gridPos = new Vector3Int(x, 0, y);

                if (emptyHolders.Contains(gridPos)) continue;

                var pos = _grid.CellToWorld(gridPos);

                var cell = _cellFactory.Create();
                cell.SetPosition(pos);

                _cellsDic[gridPos] = cell;
            }
        }

        return _cellsDic;
    }


    public void DestroyCell(ICell cell)
    {
        foreach (var keyValuePair in _cellsDic)
        {
            if (cell.Equals(keyValuePair.Value))
            {
                cell.Destroy();
                _cellsDic.Remove(keyValuePair.Key);
                break;
            }
        }
    }
}