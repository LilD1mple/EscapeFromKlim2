using EFK2.Game.ScreenService;
using EFK2.Game.ScreenService.Interfaces;
using Zenject;

namespace EFK2.Installers
{
    public class ScreenServiceInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindScreenService();
        }

        private void BindScreenService()
        {
            Container
                .Bind<IScreenService>()
                .To<ScreenService>()
                .FromNew()
                .AsSingle();
        }
    }
}