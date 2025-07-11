using EFK2.Game.PostProcess;
using EFK2.Game.PostProcess.Interfaces;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

namespace EFK2.Installers
{
    public class PostProcessServiceInstaller : MonoInstaller
    {
        [SerializeField] private VolumeProfile _volumeProfile;

        public override void InstallBindings()
        {
            BindPostProcessService();
        }

        private void BindPostProcessService()
        {
            Container
                .Bind<IPostProcessService>()
                .To<PostProcessService>()
                .FromNew()
                .AsSingle()
                .WithArguments(_volumeProfile);
        }
    } 
}