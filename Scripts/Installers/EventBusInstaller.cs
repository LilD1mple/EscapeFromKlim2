using EFK2.Events;
using Zenject;

namespace EFK2.Installers
{
    public class EventBusInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindEventBus();
        }

        private void BindEventBus()
        {
            Container
                .Bind<EventBus>()
                .FromNew()
                .AsSingle()
                .NonLazy();
        }
    }
}