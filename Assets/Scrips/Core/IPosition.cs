using UnityEngine;

namespace Scrips.Core
{
    public interface IPosition
    {
        public void SetPosition(Vector3 position);

        public Vector3 GetPosition();
    }
}