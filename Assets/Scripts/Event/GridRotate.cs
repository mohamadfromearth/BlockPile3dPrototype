using UnityEngine;

namespace Event
{
    public struct GridRotate : IEventData
    {
        public Quaternion Rotation;

        public GridRotate(Quaternion rotation)
        {
            Rotation = rotation;
        }
    }
}