using Scrips.Core;
using Scrips.Objects.Block;
using UnityEngine;

namespace Scrips.Objects.BlocksContainer
{
    public interface IBlockContainer : IPosition, IGameObject
    {
        public void Push(IBlock block);

        public IBlock Pop();

        public Color Color { get; set; }
    }
}