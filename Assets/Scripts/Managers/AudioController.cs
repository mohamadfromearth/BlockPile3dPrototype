using Core;
using Data;
using Event;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class AudioController
    {
        private AudioPlayer _player;
        private EventChannel _channel;
        private AudioRepository _repo;


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


        private void SubscribeToEvents()
        {
            _channel.Subscribe<BlockPush>(OnBlockPushed);
            _channel.Subscribe<BlockPushComplete>(ResetPitch);
            _channel.Subscribe<BlockDestroyComplete>(ResetPitch);
            _channel.Subscribe<BlockDestroy>(OnBlockDestroy);
        }


        private void OnBlockPushed()
        {
            Debug.Log("Block is being pushed");
            _player.SetPitch(AudioSourceType.Main, _blockPushingPitch);
            _blockPushingPitch += _repo.blockPushingPitchInterval;
            _player.SetAudioClip(AudioSourceType.Main, _repo.bubble);
            _player.Play(AudioSourceType.Main);
        }

        private void OnBlockDestroy()
        {
            _player.SetPitch(AudioSourceType.Main, _blockDestroyingPitch);
            _blockDestroyingPitch -= _repo.blockDestroyingPitchInterval;
            _player.SetAudioClip(AudioSourceType.Main, _repo.bubble);
            _player.Play(AudioSourceType.Main);
        }


        private void ResetPitch()
        {
            _blockPushingPitch = _repo.blockPushingInitialPitch;
            _blockDestroyingPitch = _repo.blockDestroyingInitialPitch;
        }
    }
}