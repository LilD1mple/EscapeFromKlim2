using EFK2.Debugs;
using UnityEngine;
using Zenject;

namespace EFK2.Installers
{
    public class DebugLoggerInstaller : MonoInstaller
    {
        [SerializeField] private DebugLogger _debugLogger;

        public override void InstallBindings()
        {
            BindLogger();
        }

        private void BindLogger()
        {
            _debugLogger.Construct();

            Container
                .Bind<DebugLogger>()
                .FromInstance(_debugLogger)
                .AsSingle();
        }
    }
}