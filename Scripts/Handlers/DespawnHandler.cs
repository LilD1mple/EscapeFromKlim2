using EFK2.Handlers.Interfaces;
using NTC.Pool;
using UnityEngine;

namespace EFK2.Handlers
{
    public sealed class DespawnHandler<T> : IDespawnHandler where T : Component
    {
        private readonly T _toDespawn;

        private readonly float _delay;

        public DespawnHandler(T toDespawn, float delay = 0f)
        {
            _toDespawn = toDespawn;

            _delay = delay;
        }

        void IDespawnHandler.Despawn()
        {
            NightPool.Despawn(_toDespawn, _delay);
        }
    }
}
