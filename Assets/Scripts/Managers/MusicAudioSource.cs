using UnityEngine;

namespace Managers
{
    public class MusicAudioSource : MonoBehaviour
    {
        private static MusicAudioSource _instance;

        [SerializeField] private AudioSource backgroundAudioSource;


        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(_instance);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        public static MusicAudioSource GetInstance() => _instance;

        public void Play() => backgroundAudioSource.Play();

        public void Pause() => backgroundAudioSource.Pause();
    }
}