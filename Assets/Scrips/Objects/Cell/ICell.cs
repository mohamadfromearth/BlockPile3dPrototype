using Scrips.Core;
using UnityEngine;

namespace Scrips.Objects.Cell
{
    public interface ICell : IPosition, IGameObject
    {
        public Color Color { get; set; }
    }
}