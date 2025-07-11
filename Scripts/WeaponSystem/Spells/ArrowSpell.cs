using EFK2.Handlers;
using EFK2.Handlers.Interfaces;
using EFK2.HealthSystem.Interfaces;
using NaughtyAttributes;
using NTC.OverlapSugar;
using NTC.Pool;
using System.Collections.Generic;
using UnityEngine;

namespace EFK2.WeaponSystem.Spells
{
    public class ArrowSpell : Spell
    {
        [Header("VXF Template")]
        [SerializeField] private bool _enableSpawnHit = true;
        [SerializeField, ShowIf(nameof(_enableSpawnHit))] private ParticleSystem _arrowHitTemplate;

        [Header("Settings")]
        [SerializeField] private Vector3 _rotationOffset;
        [SerializeField, Min(0)] private float _arrowHitDespawnDelay;
        [SerializeField, Min(0)] private float _offset;

        private readonly List<ParticleCollisionEvent> _collisionEvents = new(32);

        private readonly List<IDamageable> _overlapResults = new(32);

        protected override void OnParticleCollided(GameObject other)
        {
            int numCollisionEvents = SelfParticleVXF.GetCollisionEvents(other, _collisionEvents);

            for (int i = 0; i < numCollisionEvents; i++)
            {
                Vector3 hitPosition = _collisionEvents[i].intersection + _collisionEvents[i].normal * _offset;

                PerformAttack(ref hitPosition);

                SpawnArrowHit(ref hitPosition);
            }
        }

        private void SpawnArrowHit(ref Vector3 hitPosition)
        {
            if (_enableSpawnHit == false)
                return;

            var effect = NightPool.Spawn(_arrowHitTemplate, hitPosition, Quaternion.identity);

            effect.transform.LookAt(hitPosition);

            effect.transform.rotation *= Quaternion.Euler(_rotationOffset);

            DespawnArrowHit(effect);
        }

        private void PerformAttack(ref Vector3 globalHitPosition)
        {
            Vector3 localHitPoint = transform.InverseTransformPoint(globalHitPosition);

            OverlapSettings.SetOffset(localHitPoint);

            if (OverlapSettings.TryFind(_overlapResults))
                _overlapResults.ForEach(ApplyDamage);
        }

        private void DespawnArrowHit(ParticleSystem effect)
        {
            IDespawnHandler despawnHandler = new DespawnHandler<ParticleSystem>(effect, _arrowHitDespawnDelay);

            despawnHandler.Despawn();
        }

        private void ApplyDamage(IDamageable damageable) => damageable.TakeDamage(Damage);
    }
}