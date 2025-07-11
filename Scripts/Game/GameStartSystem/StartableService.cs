using System.Collections.Generic;

namespace EFK2.Game.StartSystem
{
    public sealed class StartableService : IStartable
    {
        private readonly List<IStartable> _startables = new();

        public void Register(IStartable startable)
        {
            if (_startables.Contains(startable))
                return;

            _startables.Add(startable);
        }

        public void Unregister(IStartable startable)
        {
            if (_startables.Contains(startable) == false)
                return;

            _startables.Remove(startable);
        }

        public void StartGame()
        {
            for (int i = 0; i < _startables.Count; i++)
                _startables[i].StartGame();
        }
    }
}
