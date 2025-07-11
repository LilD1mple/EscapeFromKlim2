using EFK2.Game.PauseSystem;
using Zenject;

namespace EFK2.Installers
{
    public class PauseServiceInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindPauseService();
        }

        private void BindPauseService()
        {
            Container
                .Bind<PauseService>()
                .FromNew()
                .AsSingle();
        }
    }
}