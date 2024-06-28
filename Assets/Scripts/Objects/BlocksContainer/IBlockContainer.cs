using System.Collections.Generic;
using Core;
using Scrips.Core;
using Scrips.Objects.Block;
using UnityEngine;

namespace Scrips.Objects.BlocksContainer
{
    public interface IBlockContainer : IPosition, IGameObject
    {
        public void Push(IBlock block);

        public void Push(IBlock block, float duration);

        public IBlock Pop();

        public IBlock Peek();

        public bool HasSingleColor { get; }

        public Stack<Color> Colors { get; }

        public bool IsPlaced { set; }
    }
}