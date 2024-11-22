using Event;
using UnityEngine;
using Zenject;

namespace Objects.Block
{
    public class BlockFactory : IBlockFactory
    {
        private Block[] _blockPrefabs;
        [Inject] private EventChannel _channel;


        public BlockFactory(Block[] blockPrefabs)
        {
            _blockPrefabs = blockPrefabs;
        }


        public IBlock Create(int index)
        {
            var block = Object.Instantiate(_blockPrefabs[index]);
            block.Channel = _channel;
            return block;
        }
    }
}