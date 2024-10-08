using DG.Tweening;
using UnityEngine;

namespace Objects.Block
{
    public class Block : MonoBehaviour, IBlock
    {
        private Tween _scaleTween;


        private bool _hasBeenDestroyed = false;

        public void SetPosition(Vector3 position)
        {
            if (_hasBeenDestroyed) return;
            transform.position = position;
        }

        public Vector3 GetPosition()
        {
            if (_hasBeenDestroyed) return Vector3.zero;
            return transform.position;
        }


        private Color color;

        public Color Color
        {
            set { color = value; }
            get { return color; }
        }

        public void Destroy()
        {
            _scaleTween = transform.DOScale(Vector3.zero, 0.5f);
            _scaleTween.onComplete = () => { Destroy(gameObject); };
        }

        private void OnDestroy()
        {
            _hasBeenDestroyed = true;
            _scaleTween?.Kill();
        }

        public GameObject GameObj
        {
            get
            {
                if (_hasBeenDestroyed) return null;
                return gameObject;
            }
        }
    }
}