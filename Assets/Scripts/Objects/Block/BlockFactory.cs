using UnityEngine;

namespace Objects.Block
{
    public class BlockFactory : IBlockFactory
    {
        private Block[] _blockPrefabs;


        public BlockFactory(Block[] blockPrefabs)
        {
            _blockPrefabs = blockPrefabs;
        }


        public IBlock Create(int index) => Object.Instantiate(_blockPrefabs[index]);
    }
}