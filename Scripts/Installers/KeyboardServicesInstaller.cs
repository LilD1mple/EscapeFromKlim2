using EFK2.Inputs;
using EFK2.Inputs.Interfaces;
using Zenject;

namespace EFK2.Installers
{
    public class KeyboardServicesInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindKeyboardInput();
        }

        private void BindKeyboardInput()
        {
            Container
                .Bind<IKeyboardInputService>()
                .To<KeyboardInputService>()
                .FromNew()
                .AsSingle();
        }
    }
}