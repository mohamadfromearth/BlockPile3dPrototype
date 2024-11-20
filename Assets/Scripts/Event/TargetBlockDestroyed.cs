using UnityEngine;

namespace Event
{
    public struct TargetBlockDestroyed : IEventData
    {
        public Vector3 Position;
        public int Count;
        public int ColorIndex;

        public TargetBlockDestroyed(Vector3 position, int count, int colorIndex)
        {
            Position = position;
            Count = count;
            ColorIndex = colorIndex;
        }
    }
}