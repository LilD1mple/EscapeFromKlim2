using EFK2.Events.Interfaces;
using EFK2.HealthSystem;

namespace EFK2.Events.Signals
{
    public readonly struct HealItemsCountChangedSignal : IEvent
    {
        public HealItemsCountChangedSignal(HealthItemCollectable healthItem)
        {
            HealthItem = healthItem;
        }

        public HealthItemCollectable HealthItem { get; }
    }
}
