using System.Collections.Generic;
using Objects.BlocksContainer;
using Objects.Cell;
using UnityEngine;
using Zenject;

public class Board
{
    private readonly Grid _grid;

    public int Width { get; private set; }
    public int Height { get; private set; }


    private ICellFactory _cellFactory;


    private Dictionary<Vector3Int, ICell> _cellsDic = new();


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

    public void AddBlockContainer(IBlockContainer blockContainer, Vector3 worldPosition)
    {
        var gridPos = _grid.WorldToCell(worldPosition);

        if (_cellsDic.TryGetValue(gridPos, out var cell))
        {
            if (blockContainer == null)
                cell.CanPlaceItem = true;
            else
                cell.CanPlaceItem = false;


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


    public Vector3Int WorldToCell(Vector3 worldPosition) => _grid.WorldToCell(worldPosition);

    public Vector3 CellToWorld(Vector3Int cellPosition) => _grid.CellToWorld(cellPosition);


    public void Clear()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int z = 0; z < Height; z++)
            {
                var position = new Vector3Int(x, 0, z);
                var cell = GetCell(position);

                if (cell != null && cell.BlockContainer != null)
                {
                    cell.CanPlaceItem = true;
                    cell.BlockContainer.Destroy(true);
                    cell.BlockContainer = null;
                }

                if (cell != null) cell.Destroy();
            }
        }

        _cellsDic.Clear();
    }


    public void SpawnCells(List<Vector3Int> emptyHolders, int width, int height)
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
    }
}