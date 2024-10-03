using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "So/SettingsRepository", fileName = "SettingsRepo")]
    public class SettingsRepository : ScriptableObject
    {
        [SerializeField] private Sprite soundOnSprite;
        [SerializeField] private Sprite soundOffSprite;
        [SerializeField] private Sprite musicOnSprite;
        [SerializeField] private Sprite musicOffSprite;


        private bool _isSoundOn = true;
        private bool _isMusicOn = true;

        private const string SoundKey = "SOUND_KEY";
        private const string MusicKey = "MUSIC_KEY";


        private void OnEnable()
        {
            _isSoundOn = PlayerPrefs.GetInt(SoundKey, 1) == 1;
            _isMusicOn = PlayerPrefs.GetInt(MusicKey, 1) == 1;
        }

        public void ToggleSound()
        {
            _isSoundOn = !_isSoundOn;
            PlayerPrefs.SetInt(SoundKey, _isSoundOn ? 1 : 0);
        }

        public void ToggleMusic()
        {
            _isMusicOn = !_isMusicOn;
            PlayerPrefs.SetInt(MusicKey, _isMusicOn ? 1 : 0);
        }


        public Sprite GetSoundSprite()
        {
            return _isSoundOn ? soundOnSprite : soundOffSprite;
        }

        public Sprite GetMusicSprite()
        {
            return _isMusicOn ? musicOnSprite : musicOffSprite;
        }
    }
}