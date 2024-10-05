using System.Collections.Generic;
using Core;
using DG.Tweening;
using Objects.Block;
using UnityEngine;

namespace Objects.BlocksContainer
{
    public interface IBlockContainer : IPosition, IGameObject, IParent
    {
        public void Push(IBlock block);


        public void MoveTo(Vector3 position, float duration, Ease ease = Ease.Linear);


        public void MoveTo(Transform target, float duration);

        public int Count { get; }

        public void SetCountText(string text, float delay);

        public bool WasUpperColorChanged { get; set; }

        public void Push(IBlock block, float duration);

        public IBlock Pop();

        public IBlock Peek();

        public bool HasSingleColor { get; }

        public Stack<Color> Colors { get; }


        public bool IsPlaced { set; get; }

        public float Destroy(bool destroyImmediately = false);

        public float DestroyAll();
    }
}