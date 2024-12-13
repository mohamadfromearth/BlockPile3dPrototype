using System;
using Core;
using Data;
using Event;
using Event.Coin;
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
            channel.Subscribe<CoinCollectionAnimationCompleted>(OnCoinCollecting);
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

        protected void OnCoinCollecting()
        {
            if (settingsRepository.IsSoundOn)
            {
                Player.SetAudioClip(AudioSourceType.Main, Repo.coinCollect);
                Player.Play(AudioSourceType.Main);
            }
        }
    }
}