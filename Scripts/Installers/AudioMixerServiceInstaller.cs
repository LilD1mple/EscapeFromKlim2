using EFK2.Audio;
using EFK2.Audio.Interfaces;
using UnityEngine;
using Zenject;

namespace EFK2.Installers
{
    public class AudioMixerServiceInstaller : MonoInstaller
    {
        [SerializeField] private AudioMixerService _mixerService;

        public override void InstallBindings()
        {
            BindAudioService();
        }

        private void BindAudioService()
        {
            Container
                .Bind<IAudioMixerService>()
                .To<AudioMixerService>()
                .FromInstance(_mixerService)
                .AsSingle();
        }
    } 
}