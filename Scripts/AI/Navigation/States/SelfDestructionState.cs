using EFK2.AI.Interfaces;
using EFK2.Events;
using EFK2.Extensions;
using EFK2.Handlers;
using EFK2.Handlers.Interfaces;
using EFK2.HealthSystem;
using EFK2.WeaponSystem.PostEffects;
using NTC.ContextStateMachine;
using NTC.Pool;
using System;
using UnityEngine;

namespace EFK2.AI.States
{
    public sealed class SelfDestructionState : State
    {
        private IDespawnHandler _projectilePostEffectDespawnHandler;

        private readonly float _explosionRadius;
        private readonly float _damage;
        private readonly float _explosionLifeTime;

        private readonly int _selfDestructionAnimationHash = Animator.StringToHash("Self-distruction");

        private readonly Transform _enemyTransform;
        private readonly Transform _explosionPoint;

        private readonly Health _playerHealth;

        private readonly EventBus _eventBus;

        private readonly ProjectilePostEffect _projectilePostEffect;

        private readonly Action _warriorDead;

        private readonly INavigationAnimatorService _navigationAnimatorService;

        public SelfDestructionState(INavigationAnimatorService navigationAnimatorService, ProjectilePostEffect projectilePostEffect, Transform enemyTransform, Transform explosionPoint, Health health, EventBus eventBus, Action warriorDead, float explosionRadius, float damage, float explosionLifeTime)
        {
            _navigationAnimatorService = navigationAnimatorService;

            _projectilePostEffect = projectilePostEffect;

            _enemyTransform = enemyTransform;

            _playerHealth = health;

            _eventBus = eventBus;

            _warriorDead = warriorDead;

            _explosionRadius = explosionRadius;

            _damage = damage;

            _explosionLifeTime = explosionLifeTime;

            _explosionPoint = explosionPoint;
        }

        public override void OnEnter()
        {
            _navigationAnimatorService.SetTrigger(_selfDestructionAnimationHash);

            PerformDamage();

            SpawnExplosion();

            _warriorDead?.Invoke();
        }

        private void SpawnExplosion()
        {
            var effect = NightPool.Spawn(_projectilePostEffect, _explosionPoint.position, Quaternion.identity);

            _projectilePostEffectDespawnHandler = new DespawnHandler<ProjectilePostEffect>(effect, _explosionLifeTime);

            _projectilePostEffectDespawnHandler.Despawn();
        }

        private void PerformDamage()
        {
            if (_enemyTransform.CheckMaxDistanceBetweenTwoTransforms(_playerHealth.transform, _explosionRadius))
                _playerHealth.DamagePlayer(_eventBus, _damage);
        }
    }
}
