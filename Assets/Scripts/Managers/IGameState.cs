using UnityEngine;

namespace Managers
{
    public interface IGameState
    {
        public void OnEnter();
        public void OnExit();


        public void OnPointerMove(Vector3 position);


        public void OnContainerPointerDown();
    }
}