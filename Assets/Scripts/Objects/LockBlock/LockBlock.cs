using System.Collections;
using Data;
using Event;
using TMPro;
using UnityEngine;

namespace Objects.LockBlock
{
    public class LockBlock : MonoBehaviour, ILockBlock
    {
        [SerializeField] private TextMeshPro text;

        [SerializeField] private MeshRenderer renderer;

        [SerializeField] private ParticleSystem explosionParticle;

        [SerializeField] private SettingsRepository settingRepo;

        [SerializeField] private AudioSource explosionAudioSource;


        private EventChannel _channel;

        public EventChannel Channel
        {
            get => _channel;
            set
            {
                _channel = value;
                _channel.Subscribe<ScoreChanged>(OnScoreChanged);
                _channel.Subscribe<GridRotate>(OnGridRotate);
            }
        }


        private void OnDisable()
        {
            _channel.UnSubscribe<ScoreChanged>(OnScoreChanged);
            _channel.UnSubscribe<GridRotate>(OnGridRotate);
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

        public void Destroy() => StartCoroutine(DestroyRoutine());


        private IEnumerator DestroyRoutine()
        {
            explosionParticle.Play();
            renderer.enabled = false;
            text.enabled = false;
            if (settingRepo.IsSoundOn) explosionAudioSource.Play();
            yield return new WaitForSeconds(0.5f);
            Destroy(gameObject);
        }

        private void OnScoreChanged()
        {
            var score = Channel.GetData<ScoreChanged>().Score;

            if (score >= _count)
            {
                Channel.Rise<ScoreHitLockBLock>(new ScoreHitLockBLock(this));
            }
        }


        private void OnGridRotate()
        {
            //if (_hasBeenDestroyed) return;
            var rotation = Quaternion.Inverse(Channel.GetData<GridRotate>().Rotation);
            transform.rotation = Quaternion.Euler(0, rotation.y, 0);
        }


        private int _count;
        public GameObject GameObj => gameObject;
    }
}