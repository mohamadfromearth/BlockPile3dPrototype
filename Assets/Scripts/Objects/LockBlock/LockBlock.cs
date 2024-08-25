using Event;
using TMPro;
using UnityEngine;

namespace Objects.LockBlock
{
    public class LockBlock : MonoBehaviour, ILockBlock
    {
        [SerializeField] private TextMeshPro text;


        private EventChannel _channel;

        public EventChannel Channel
        {
            get => _channel;
            set
            {
                _channel = value;
                _channel.Subscribe<ScoreChanged>(OnScoreChanged);
            }
        }


        private void OnDisable()
        {
            _channel.UnSubscribe<ScoreChanged>(OnScoreChanged);
        }


        public void SetPosition(Vector3 position) => transform.position = position;


        public Vector3 GetPosition() => transform.position;

        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                text.text = _count.ToString();
            }
        }

        public void Destroy() => Destroy(gameObject);

        private void OnScoreChanged()
        {
            var score = Channel.GetData<ScoreChanged>().Score;

            if (score >= _count)
            {
                Channel.Rise<ScoreHitLockBLock>(new ScoreHitLockBLock(this));
            }
        }


        private int _count;
        public GameObject GameObj => gameObject;
    }
}