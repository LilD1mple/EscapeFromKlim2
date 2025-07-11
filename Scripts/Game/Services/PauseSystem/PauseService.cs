using System.Collections.Generic;

namespace EFK2.Game.PauseSystem
{
    public sealed class PauseService : IPauseable
    {
        private readonly List<IPauseable> _pauseables = new(16);

        public bool IsPaused { get; private set; } = false;

        public void Register(IPauseable pauseable)
        {
            if (_pauseables.Contains(pauseable))
                return;

            _pauseables.Add(pauseable);
        }

        public void Unregister(IPauseable pauseable)
        {
            if (_pauseables.Contains(pauseable) == false)
                return;

            _pauseables.Remove(pauseable);
        }

        public void SetPause(bool isPaused)
        {
            IsPaused = isPaused;

            for (int i = 0; i < _pauseables.Count; i++)
                _pauseables[i].SetPause(isPaused);
        }
    }
}
