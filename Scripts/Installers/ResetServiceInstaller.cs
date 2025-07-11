using EFK2.Game.ResetSystem;
using Zenject;

namespace EFK2.Installers
{
    public class ResetServiceInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindResetService();
        }

        private void BindResetService()
        {
            Container
                .Bind<ResetService>()
                .FromNew()
                .AsSingle();
        }
    }
}