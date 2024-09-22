using UnityEngine;

namespace Event
{
    public struct TargetBlockDestroyed : IEventData
    {
        public Vector3 Position;
        public int Count;

        public TargetBlockDestroyed(Vector3 position, int count)
        {
            Position = position;
            Count = count;
        }
    }
}