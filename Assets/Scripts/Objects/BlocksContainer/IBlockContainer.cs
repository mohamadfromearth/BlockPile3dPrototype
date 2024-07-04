using System.Collections.Generic;
using Objects.Block;
using Scrips.Core;
using UnityEngine;

namespace Objects.BlocksContainer
{
    public interface IBlockContainer : IPosition, IGameObject
    {
        public void Push(IBlock block);


        public int Count { get; }

        public void Push(IBlock block, float duration);

        public IBlock Pop();

        public IBlock Peek();

        public bool HasSingleColor { get; }

        public Stack<Color> Colors { get; }

        public bool IsPlaced { set; }

        public void Destroy();
    }
}