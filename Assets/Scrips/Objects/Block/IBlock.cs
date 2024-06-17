using Scrips.Core;
using UnityEngine;

namespace Scrips.Objects.Cell
{
    public interface IBlock : IPosition, IGameObject
    {
        public Color Color { get; set; }
    }
}