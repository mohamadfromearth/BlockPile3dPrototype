using Scrips.Core;
using Scrips.Objects.Cell;
using UnityEngine;

namespace Scrips.Objects.CellsContainer
{
    public interface IBlockContainer : IPosition, IGameObject
    {
        public void Push(IBlock block);

        public IBlock Pop();

        public Color Color { get; set; }
    }
}