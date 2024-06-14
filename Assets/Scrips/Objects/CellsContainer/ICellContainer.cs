using Scrips.Core;
using Scrips.Objects.Cell;
using UnityEngine;

namespace Scrips.Objects.CellsContainer
{
    public interface ICellContainer : IPosition, IGameObject
    {
        public void Push(ICell cell);

        public ICell Pop();

        public Color Color { get; set; }
    }
}