using Core;
using Data;
using Event;
using TMPro;
using UI;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class GameAudioController : AudioController
    {
        [SerializeField] private GameUI gameUI;

        private float _blockPushingPitch;
        private float _blockDestroyingPitch;


        [Inject]
        public override void Construct(AudioPlayer player, EventChannel channel, AudioRepository repo)
        {
            base.Construct(player, channel, repo);
            SubscribeToEvents();
        }


        private void OnDisable()
        {
            gameUI.RemoveCoinCollectionAnimationCompleteListener(OnCoinCollecting);
        }


        private void SubscribeToEvents()
        {
            Channel.Subscribe<BlockPush>(OnBlockPushed);
            Channel.Subscribe<BlockPushComplete>(ResetPitch);
            Channel.Subscribe<BlockContainerDestroy>(ResetPitch);
            Channel.Subscribe<BlockDestroy>(OnBlockDestroy);
            Channel.Subscribe<CellContainerPointerDown>(OnBlockContainerPointerDown);
            Channel.Subscribe<UpdateBoardCompleted>(ResetPitch);

            gameUI.AddCoinCollectionAnimationCompleteListener(OnCoinCollecting);
        }


        private void OnBlockPushed()
        {
            if (settingsRepository.IsSoundOn)
            {
                Player.SetPitch(AudioSourceType.Main, _blockPushingPitch);
                Player.SetAudioClip(AudioSourceType.Main, Repo.bubble);
                Player.Play(AudioSourceType.Main);
            }

            _blockPushingPitch += Repo.blockPushingPitchInterval;
        }

        private void OnBlockDestroy()
        {
            Player.SetPitch(AudioSourceType.Main, _blockDestroyingPitch);
            _blockDestroyingPitch -= Repo.blockDestroyingPitchInterval;

            if (settingsRepository.IsSoundOn)
            {
                Player.SetAudioClip(AudioSourceType.Main, Repo.bubble);
                Player.Play(AudioSourceType.Main);
            }
        }


        private void OnCoinCollecting()
        {
            if (settingsRepository.IsSoundOn)
            {
                Player.SetAudioClip(AudioSourceType.Main, Repo.coinCollect);
                Player.Play(AudioSourceType.Main);
            }
        }


        private void OnBlockContainerPointerDown()
        {
            var data = Channel.GetData<CellContainerPointerDown>();
            if (settingsRepository.IsSoundOn && data.BlockContainer.IsPlaced == false)
            {
                Player.SetAudioClip(AudioSourceType.Second, Repo.blockPickUp);
                Player.Play(AudioSourceType.Second);
            }
        }


        private void ResetPitch()
        {
            _blockPushingPitch = Repo.blockPushingInitialPitch;
            _blockDestroyingPitch = Repo.blockDestroyingInitialPitch;
        }
    }
}