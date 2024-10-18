using System.Collections.Generic;
using DG.Tweening;
using Event;
using Objects.AdvertiseBlock;
using Objects.BlocksContainer;
using Objects.Cell;
using Objects.LockBlock;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class Board
{
    private readonly Grid _grid;
    private readonly Transform _pivot;
    private readonly float[] _snapAngles = { 0, 90f, 180f, 270f, 360f };

    public int Width { get; private set; }
    public int Height { get; private set; }

    public Transform Center => _pivot;


    private ICellFactory _cellFactory;


    private Dictionary<Vector3Int, ICell> _cellsDic = new();


    public Dictionary<Vector3Int, ICell> Cells => _cellsDic;

    private EventChannel _channel;


    private ShuffleHandler _shuffleHandler = new ShuffleHandler();


    private int _filledCellItemsCount;


    private bool _isRotationSnapping = false;

    public bool IsRotationSnapping => _isRotationSnapping;

    public bool IsFilled => _filledCellItemsCount >= _cellsDic.Count;

    public int FilledCellItemsCount => _filledCellItemsCount;

    public int CellsItemCount => _cellsDic.Count;

    public Board(int width, int height, Grid grid, Transform pivot)
    {
        _grid = grid;
        _pivot = pivot;
    }

    [Inject]
    private void Construct(ICellFactory cellFactory, EventChannel channel)
    {
        _cellFactory = cellFactory;
        _channel = channel;
    }


    public ICell GetCell(Vector3 worldPosition)
    {
        var gridPos = WorldToCell(worldPosition);
        if (_cellsDic.TryGetValue(gridPos, out var holder))
        {
            return holder;
        }

        return null;
    }


    public ICell GetCell(Vector3Int boardPosition)
    {
        Debug.Log("Board position from GetCell is " + boardPosition);
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
        var gridPos = WorldToCell(worldPosition);


        if (_cellsDic.TryGetValue(gridPos, out var cell))
        {
            _filledCellItemsCount += BoardHelpers.GetItemsCountModifier(blockContainer, cell);
            cell.CanPlaceItem = blockContainer == null;
            cell.BlockContainer = blockContainer;

            _shuffleHandler.Add(gridPos);
        }
    }


    public void AddBlockContainer(IBlockContainer blockContainer, Vector3Int gridPosition)
    {
        if (_cellsDic.TryGetValue(gridPosition, out var cell))
        {
            _filledCellItemsCount += BoardHelpers.GetItemsCountModifier(blockContainer, cell);

            cell.CanPlaceItem = blockContainer == null;
            cell.BlockContainer = blockContainer;
            cell.BlockContainer = blockContainer;
            blockContainer?.SetPosition(cell.GetPosition());
            _shuffleHandler.Add(gridPosition);
        }
    }

    public void AddAdvertiseBlock(IAdvertiseBlock advertiseBlock, Vector3Int gridPosition)
    {
        if (_cellsDic.TryGetValue(gridPosition, out var cell))
        {
            _filledCellItemsCount += BoardHelpers.GetItemsCountModifier(advertiseBlock, cell);

            cell.CanPlaceItem = advertiseBlock == null;

            cell.AdvertiseBlock = advertiseBlock;

            if (cell.CanPlaceItem) _shuffleHandler.Add(gridPosition);
            else _shuffleHandler.Remove(gridPosition);
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
            _filledCellItemsCount += BoardHelpers.GetItemsCountModifier(lockBlock, cell);

            cell.LockBlock = lockBlock;
            cell.CanPlaceItem = lockBlock == null;

            if (cell.CanPlaceItem) _shuffleHandler.Add(gridPosition);
            else _shuffleHandler.Remove(gridPosition);
        }
    }


    public Vector3 GetBoardCenter()
    {
        int halfWidth = Width / 2;
        Vector3Int centerCell = new Vector3Int(halfWidth, 0, halfWidth);
        Vector3 centerPos = CellToWorld(centerCell);
        if (Width % 2 == 0)
        {
            centerPos.x -= 0.55f;
            centerPos.z -= 0.55f;
        }

        return centerPos;
    }


    public void Rotate(float amount)
    {
        var lastRotation = _pivot.transform.rotation.eulerAngles;
        _pivot.transform.rotation = Quaternion.Euler(lastRotation.x, lastRotation.y + amount, lastRotation.z);
    }

    public void SnapRotation()
    {
        _isRotationSnapping = true;
        float currentAngle = _pivot.transform.rotation.eulerAngles.y;
        float closestAngle = _snapAngles[0];
        float minDifference = Mathf.Abs(currentAngle - closestAngle);

        foreach (float angle in _snapAngles)
        {
            float difference = Mathf.Abs(currentAngle - angle);
            if (difference < minDifference)
            {
                minDifference = difference;
                closestAngle = angle;
            }
        }

        var rotationTween = _pivot.transform.DORotate(new Vector3(0, closestAngle, 0), 0.5f);

        rotationTween.onUpdate = () => { _channel.Rise<GridRotate>(new GridRotate(_pivot.transform.rotation)); };

        rotationTween.onComplete = () => { _isRotationSnapping = false; };
    }


    public Vector3Int WorldToCell(Vector3 worldPosition)
    {
        var gridPos = _grid.WorldToCell(worldPosition);

        return gridPos;
    }

    public Vector3Int WordToCellUnRotated(Vector3 worldPosition)
    {
        return _grid.WorldToCell(worldPosition);
    }

    public Vector3Int WorldToCell2(Vector3 worldPosition)
    {
        var gridPos = _grid.WorldToCell(worldPosition);


        return gridPos;
    }


    public Vector3 CellToWorld(Vector3Int cellPosition)
    {
        return _grid.CellToWorld(cellPosition);
    }


    public void Clear()
    {
        _filledCellItemsCount = 0;

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
        _shuffleHandler.Clear();
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

                _shuffleHandler.Add(gridPos);
            }
        }

        _pivot.rotation = Quaternion.identity;
        _grid.transform.SetParent(null);
        _pivot.transform.position = GetBoardCenter();
        _grid.transform.SetParent(_pivot);
    }


    public float Shuffle()
    {
        var index = 0;
        var blockContainers = new List<KeyValuePair<Vector3Int, IBlockContainer>>();
        var shuffledPosition = _shuffleHandler.GetShuffledPositions();


        foreach (var keyValuePair in _cellsDic)
        {
            var container = keyValuePair.Value.BlockContainer;
            if (container != null)
            {
                var newPos = shuffledPosition[index];

                blockContainers.Add(new(newPos, container));
                keyValuePair.Value.BlockContainer = null;
                keyValuePair.Value.CanPlaceItem = true;
                index++;
            }
        }


        foreach (var keyValuePair in blockContainers)
        {
            _cellsDic[keyValuePair.Key].BlockContainer = keyValuePair.Value;
            _cellsDic[keyValuePair.Key].CanPlaceItem = false;
            keyValuePair.Value.MoveTo(_cellsDic[keyValuePair.Key].GameObj.transform, 0.15f);
        }

        return 0.5f;
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