using EFK2.Audio;
using EFK2.Audio.Interfaces;
using UnityEngine;
using Zenject;

namespace EFK2.Installers
{
    public class BackgroundMusicServiceInstaller : MonoInstaller
    {
        [SerializeField] private BackgroundMusicService _backgroundMusic;

        public override void InstallBindings()
        {
            BindMusicService();
        }

        private void BindMusicService()
        {
            Container
                .Bind<IMusicService>()
                .To<BackgroundMusicService>()
                .FromInstance(_backgroundMusic)
                .AsSingle();
        }
    } 
}