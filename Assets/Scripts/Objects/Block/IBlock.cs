using Core;
using UnityEngine;

namespace Objects.Block
{
    public interface IBlock : IPosition, IGameObject
    {
        public Color Color { get; set; }

        public int ColorIndex { get; set; }

        public void Destroy();
    }
}