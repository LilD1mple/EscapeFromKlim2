using System.Collections.Generic;

namespace EFK2.Game.ResetSystem
{
    public sealed class ResetService : IResetable
    {
        private readonly List<IResetable> _resetables = new();

        public void Register(IResetable resetable) => _resetables.Add(resetable);

        public void Unregister(IResetable resetable) => _resetables.Remove(resetable);

        public void Reset()
        {
            for (int i = 0; i < _resetables.Count; i++)
                _resetables[i].Reset();
        }
    }
}
