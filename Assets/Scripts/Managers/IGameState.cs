using UnityEngine;

namespace Managers
{
    public interface IGameState
    {
        public void OnPointerMove(Vector3 position);
    }
}