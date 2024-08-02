using Object = UnityEngine.Object;

namespace Objects.Block
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