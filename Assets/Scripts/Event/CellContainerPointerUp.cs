﻿using Event;
using Objects.BlocksContainer;

namespace Scrips.Event
{
    public struct CellContainerPointerUp : IEventData
    {
        public IBlockContainer BlockContainer { get; }

        public CellContainerPointerUp(IBlockContainer blockContainer)
        {
            BlockContainer = blockContainer;
        }
    }
}