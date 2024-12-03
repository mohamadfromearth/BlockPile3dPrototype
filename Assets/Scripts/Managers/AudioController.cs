using System;
using Core;
using Data;
using Event;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class AudioController : MonoBehaviour
    {
        protected AudioPlayer Player;
        protected EventChannel Channel;
        [SerializeField] protected AudioRepository Repo;
        [SerializeField] protected SettingsRepository settingsRepository;


        [Inject]
        public virtual void Construct(AudioPlayer player, EventChannel channel)
        {
            Player = player;
            Channel = channel;
            settingsRepository.AddMusicToggleListener(OnMusicToggle);
        }


        private void OnMusicToggle()
        {
            var source = MusicAudioSource.GetInstance();

            if (settingsRepository.IsMusicOn)
            {
                source.Play();
            }
            else
            {
                source.Pause();
            }
        }
    }
}