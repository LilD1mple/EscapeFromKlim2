using EFK2.Game.PauseSystem;
using EFK2.Game.UpdateSystem.Interfaces;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace EFK2.Game.UpdateSystem
{
    public sealed class GlobalUpdate : MonoBehaviour, IPauseable
    {
        [Inject] private readonly PauseService _pauseService;

        private readonly List<IRunSystem> _runSystems = new();

        public bool IsPaused { get; private set; } = false;

        public void RegistRunSystem(IRunSystem runSystem)
        {
            if (_runSystems.Contains(runSystem))
                return;

            _runSystems.Add(runSystem);
        }

        public void UnregistRunSystem(IRunSystem runSystem)
        {
            if (_runSystems.Contains(runSystem) == false) 
                return;

            _runSystems.Remove(runSystem);
        }

        private void OnEnable()
        {
            _pauseService.Register(this);
        }

        private void OnDisable()
        {
            _pauseService.Unregister(this);
        }

        private void Update()
        {
            if (IsPaused)
                return;

            for (int i = 0; i < _runSystems.Count; i++)
                _runSystems[i].Run();
        }

        void IPauseable.SetPause(bool isPaused)
        {
            IsPaused = isPaused;
        }
    }
}
