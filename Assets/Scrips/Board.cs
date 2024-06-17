using System.Collections.Generic;
using Scrips.Objects.BlockContainerHolder;
using Scrips.Objects.CellsContainer;
using UnityEngine;
using Zenject;

namespace Scrips
{
    public class Board
    {
        private Grid _grid;

        private int _width, _height;

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
            SpawnHolders();
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


        private void SpawnHolders()
        {
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    var gridPos = new Vector3Int(x, 0, y);
                    var pos = _grid.CellToWorld(gridPos);


                    var holder = _blockContainerHolderFactory.Create();
                    holder.SetPosition(pos);

                    _blockContainerHoldersDic[gridPos] = holder;
                }
            }
        }
    }
}