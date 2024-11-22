using UnityEngine;

namespace Event
{
    public struct BlockContainerDestroy : IEventData
    {
        public readonly int Count;
        public Vector3 Position;

        public BlockContainerDestroy(int count, Vector3 position)
        {
            Count = count;
            Position = position;
        }
    }
}