using Scrips.Core;
using UnityEngine;

namespace Objects.Block
{
    public interface IBlock : IPosition, IGameObject
    {
        public Color Color { get; set; }

        public void Destroy();
    }
}