using System;
using Objects.Block;
using Object = UnityEngine.Object;

namespace Scrips.Objects.Cell
{
    public class BlockFactory : IBlockFactory
    {
        private Block _blockPrefab;


        public BlockFactory(Block blockPrefab)
        {
            _blockPrefab = blockPrefab;
        }


        public IBlock Create() => Object.Instantiate(_blockPrefab);
    }
}