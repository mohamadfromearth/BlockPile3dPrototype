using Core;
using Data;
using Event;
using UnityEngine;
using Zenject;

namespace Di
{
    public class MenuInstaller : MonoInstaller
    {
        [SerializeField] private AudioPlayerData[] audioPlayerDataList;

        [SerializeField] private LevelRepository levelRepository;


        public override void InstallBindings()
        {
            Container.Bind<EventChannel>().AsSingle().NonLazy();

            Container.Bind<AudioPlayer>().AsSingle().WithArguments(audioPlayerDataList).NonLazy();


            Container.Bind<ILevelRepository>().FromInstance(levelRepository).AsSingle().NonLazy();
        }
    }
}