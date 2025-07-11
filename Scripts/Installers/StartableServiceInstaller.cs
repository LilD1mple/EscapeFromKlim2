using EFK2.Game.StartSystem;
using Zenject;

namespace EFK2.Installers
{
    public class StartableServiceInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindStartableService();
        }

        private void BindStartableService()
        {
            Container
                .Bind<StartableService>()
                .FromNew()
                .AsSingle()
                .NonLazy();
        }
    }
}