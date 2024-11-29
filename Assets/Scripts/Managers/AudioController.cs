using System;
using Core;
using Data;
using Event;
using UI;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class AudioController : MonoBehaviour
    {
        private AudioPlayer _player;
        private EventChannel _channel;
        private AudioRepository _repo;
        [SerializeField] private SettingsRepository settingsRepository;
        [SerializeField] private GameUI gameUI;

        private float _blockPushingPitch;
        private float _blockDestroyingPitch;


        [Inject]
        public void Construct(AudioPlayer player, EventChannel channel, AudioRepository repo)
        {
            _player = player;
            _channel = channel;
            _repo = repo;
            _blockPushingPitch = _repo.blockPushingInitialPitch;
            _blockDestroyingPitch = _repo.blockDestroyingInitialPitch;

            SubscribeToEvents();
        }

        private void OnDisable()
        {
            gameUI.RemoveCoinCollectionAnimationCompleteListener(OnCoinCollecting);
        }


        private void SubscribeToEvents()
        {
            _channel.Subscribe<BlockPush>(OnBlockPushed);
            _channel.Subscribe<BlockPushComplete>(ResetPitch);
            _channel.Subscribe<BlockDestroyComplete>(ResetPitch);
            _channel.Subscribe<BlockDestroy>(OnBlockDestroy);

            gameUI.AddCoinCollectionAnimationCompleteListener(OnCoinCollecting);
        }


        private void OnBlockPushed()
        {
            if (settingsRepository.IsSoundOn)
            {
                _player.SetPitch(AudioSourceType.Main, _blockPushingPitch);
                _player.SetAudioClip(AudioSourceType.Main, _repo.bubble);
                _player.Play(AudioSourceType.Main);
            }

            _blockPushingPitch += _repo.blockPushingPitchInterval;
        }

        private void OnBlockDestroy()
        {
            _player.SetPitch(AudioSourceType.Main, _blockDestroyingPitch);
            _blockDestroyingPitch -= _repo.blockDestroyingPitchInterval;

            if (settingsRepository.IsSoundOn)
            {
                _player.SetAudioClip(AudioSourceType.Main, _repo.bubble);
                _player.Play(AudioSourceType.Main);
            }
        }


        private void OnCoinCollecting()
        {
            if (settingsRepository.IsSoundOn)
            {
                _player.SetAudioClip(AudioSourceType.Main, _repo.coinCollect);
                _player.Play(AudioSourceType.Main);
            }
        }


        private void ResetPitch()
        {
            _blockPushingPitch = _repo.blockPushingInitialPitch;
            _blockDestroyingPitch = _repo.blockDestroyingInitialPitch;
        }
    }
}