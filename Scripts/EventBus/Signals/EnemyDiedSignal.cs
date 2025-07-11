using EFK2.AI.StateMachines;
using EFK2.Events.Interfaces;

namespace EFK2.Events.Signals
{
    public readonly struct EnemyDiedSignal : IEvent
    {
        public EnemyDiedSignal(EnemyStateMachine enemy)
        {
            Enemy = enemy;
        }

        public EnemyStateMachine Enemy { get; }
    }
}
