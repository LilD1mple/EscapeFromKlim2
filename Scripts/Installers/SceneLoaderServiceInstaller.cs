using EFK2.Game.SceneService;
using EFK2.Game.SceneService.Interfaces;
using UnityEngine;
using Zenject;

namespace EFK2.Installers
{
    public class SceneLoaderServiceInstaller : MonoInstaller
    {
        [SerializeField] private SceneLoaderService _sceneLoaderService;

        public override void InstallBindings()
        {
            BindSceneLoader();
        }

        private void BindSceneLoader()
        {
            Container
                .Bind<ISceneLoaderService>()
                .To<SceneLoaderService>()
                .FromInstance(_sceneLoaderService)
                .AsSingle();
        }
    } 
}