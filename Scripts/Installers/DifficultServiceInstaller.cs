using EFK2.Difficult;
using Zenject;

namespace EFK2.Installers
{
    public class DifficultServiceInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindDifficultService();
        }

        private void BindDifficultService()
        {
            Container
                .Bind<IDifficultService>()
                .To<GameDifficultService>()
                .FromNew()
                .AsSingle();
        }
    }
}