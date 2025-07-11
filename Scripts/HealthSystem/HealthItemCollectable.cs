using EFK2.Events;
using EFK2.Events.Signals;
using EFK2.Handlers;
using EFK2.Handlers.Interfaces;
using EFK2.Player;
using EFK2.Target.Interfaces;
using UnityEngine;
using Zenject;

namespace EFK2.HealthSystem
{
    [RequireComponent(typeof(Collider))]
    public class HealthItemCollectable : MonoBehaviour
    {
        [Header("Audio")]
        [SerializeField] private AudioClip _itemCollectedSFX;

        [Header("Heal")]
        [SerializeField, Min(0f)] private float _healScore;

        private Health _target;
        private EventBus _eventBus;

        private IDespawnHandler _despawnHandler;

        [Inject]
        public void Construct(ITargetService playerTarget, EventBus eventBus)
        {
            _target = playerTarget.Health;

            _eventBus = eventBus;

            _despawnHandler = new DespawnHandler<HealthItemCollectable>(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Unit unit))
                OnUnitEnter();
        }

        private void OnUnitEnter()
        {
            _target.Heal(_healScore);

            AudioSource.PlayClipAtPoint(_itemCollectedSFX, transform.position);

            RaiseSignals();

            _despawnHandler.Despawn();
        }

        private void RaiseSignals()
        {
            _eventBus.Raise(new PlayerHealthChangedSignal(_target, false));

            _eventBus.Raise(new HealItemsCountChangedSignal(this));
        }
    }
}
