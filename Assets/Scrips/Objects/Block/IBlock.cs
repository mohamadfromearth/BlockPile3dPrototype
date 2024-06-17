using Scrips.Core;
using UnityEngine;

namespace Scrips.Objects.Block
{
    public interface IBlock : IPosition, IGameObject
    {
        public Color Color { get; set; }
    }
}