using UnityEngine;

namespace Event
{
    public struct BlockDestroy : IEventData
    {
        public readonly int Count;
        public Vector3 Position;

        public BlockDestroy(int count, Vector3 position)
        {
            Count = count;
            Position = position;
        }
    }
}