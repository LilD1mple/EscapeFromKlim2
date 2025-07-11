using EFK2.Events.Interfaces;
using EFK2.HealthSystem;

namespace EFK2.Events.Signals
{
    public readonly struct PlayerHealthChangedSignal : IEvent
    {
        public PlayerHealthChangedSignal(Health health, bool damaged)
        {
            PlayerHeath = health;

            Damaged = damaged;
        }

        public Health PlayerHeath { get; }

        public bool Damaged { get; }
    }
}
