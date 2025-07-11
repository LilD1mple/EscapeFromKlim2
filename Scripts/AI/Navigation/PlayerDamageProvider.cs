using EFK2.Events;
using EFK2.Extensions;
using EFK2.HealthSystem;
using EFK2.Target.Interfaces;
using System;
using UnityEngine;
using Zenject;

namespace EFK2.AI
{
    public class PlayerDamageProvider : MonoBehaviour
    {
        private float _damage;

        private Health _target;
        private EventBus _eventBus;

        [Inject]
        public void Construct(EventBus eventBus, ITargetService playerTarget)
        {
            _target = playerTarget.Health;

            _eventBus = eventBus;
        }

        public void SetDamage(float damage)
        {
            if (damage < 0f)
                throw new ArgumentOutOfRangeException(nameof(damage));

            _damage = damage;
        }

        public void HitTarget()
        {
            _target.DamagePlayer(_eventBus, _damage);
        }
    }
}
