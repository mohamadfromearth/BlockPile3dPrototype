using System.Collections.Generic;
using Core;
using Objects.Block;
using UnityEngine;

namespace Objects.BlocksContainer
{
    public interface IBlockContainer : IPosition, IGameObject
    {
        public void Push(IBlock block);


        public int Count { get; }

        public bool WasUpperColorChanged { get; set; }

        public void Push(IBlock block, float duration);

        public IBlock Pop();

        public IBlock Peek();

        public bool HasSingleColor { get; }

        public Stack<Color> Colors { get; }

        public bool IsPlaced { set; }

        public float Destroy(bool destroyImmediately = false);
    }
}