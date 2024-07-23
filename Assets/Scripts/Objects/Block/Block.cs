using System;
using DG.Tweening;
using UnityEngine;

namespace Objects.Block
{
    public class Block : MonoBehaviour, IBlock
    {
        [SerializeField] private MeshRenderer renderer;

        private Tween scaleTween;

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public Vector3 GetPosition() => transform.position;


        private Color color;

        public Color Color
        {
            set
            {
                foreach (var material in renderer.materials)
                {
                    material.color = value;
                }

                color = value;
            }
            get { return color; }
        }

        public void Destroy()
        {
            scaleTween = transform.DOScale(Vector3.zero, 0.5f);
            scaleTween.onComplete = () => { Destroy(gameObject); };
        }

        private void OnDestroy()
        {
            scaleTween?.Kill();
        }

        public GameObject GameObj
        {
            get => gameObject;
        }
    }
}