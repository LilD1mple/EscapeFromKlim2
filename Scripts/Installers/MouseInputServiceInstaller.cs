using EFK2.Inputs;
using EFK2.Inputs.Interfaces;
using Zenject;

namespace EFK2.Installers
{
    public class MouseInputServiceInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindMouseInput();
        }

        private void BindMouseInput()
        {
            Container
                .Bind<IMouseInputService>()
                .To<MouseInputService>()
                .FromNew()
                .AsSingle();
        }
    }
}