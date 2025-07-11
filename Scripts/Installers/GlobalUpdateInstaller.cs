using EFK2.Game.UpdateSystem;
using UnityEngine;
using Zenject;

namespace EFK2.Installers
{
    public class GlobalUpdateInstaller : MonoInstaller
    {
        [SerializeField] private GlobalUpdate _globalUpdate;

        public override void InstallBindings()
        {
            BindGlobalUpdate();
        }

        private void BindGlobalUpdate()
        {
            Container.Bind<GlobalUpdate>().FromInstance(_globalUpdate).AsSingle();
        }
    } 
}