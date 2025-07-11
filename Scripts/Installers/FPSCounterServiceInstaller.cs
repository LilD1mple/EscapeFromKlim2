using EFK2.Game.FPSCounter;
using EFK2.Game.FPSCounter.Interfaces;
using Zenject;

namespace EFK2.Installers
{
    public class FPSCounterServiceInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindFPSService();
        }

        private void BindFPSService()
        {
            Container
                .Bind<IFPSCounterService>()
                .To<FPSCounterService>()
                .FromNew()
                .AsSingle();
        }
    }
}