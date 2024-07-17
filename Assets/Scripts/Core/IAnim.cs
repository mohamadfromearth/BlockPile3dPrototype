using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public interface IAnim
    {
        public void Move(Vector3 position, float duration);

        public void Rotation(Vector3 rotation, float duration);

        public void MoveInPath(List<Vector3> points, float duration);
    }
}