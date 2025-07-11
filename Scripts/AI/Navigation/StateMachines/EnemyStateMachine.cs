using EFK2.Difficult.Configurations;
using EFK2.Enums;
using EFK2.Events;
using EFK2.Events.Interfaces;
using EFK2.Events.Signals;
using EFK2.Game.PauseSystem;
using EFK2.Handlers;
using EFK2.Handlers.Interfaces;
using EFK2.HealthSystem;
using EFK2.HealthSystem.Interfaces;
using NTC.ContextStateMachine;
using NTC.Pool;
using Pathfinding;
using System;
using UnityEngine;

namespace EFK2.AI.StateMachines
{
	[SelectionBase]
	[RequireComponent(typeof(AIPath), typeof(NavigationAnimatorService))]
	public abstract class EnemyStateMachine : MonoBehaviour, IPauseable, IPoolable, IDamageable, IEventReceiver<PlayerDiedSignal>
	{
		[Header("Main")]
		[SerializeField] private Health _health;

		[Header("Animator")]
		[SerializeField] private NavigationAnimatorService _navigationAnimatorService;

		[Header("Death Settings")]
		[SerializeField, Min(0f)] private float _delayBeforeDepsawn;

		[Header("Navigation")]
		[SerializeField] private EnemyMovementFlags _enemyMovementStates = EnemyMovementFlags.Run;

		private float _runSpeed = 2.5f;
		private float _maxAttackDistance = 5f;

		private readonly StateMachine _stateMachine = new();

		private IDespawnHandler _despawnHandler;

		public bool HisAlive => _health.IsDead != true;

		protected float RunSpeed => _runSpeed;

		protected float MaxAttackDistance => _maxAttackDistance;

		protected StateMachine StateMachine => _stateMachine;

		protected Health Health => _health;

		protected NavigationAnimatorService NavigationAnimator => _navigationAnimatorService;

		protected EnemyMovementFlags EnemyMovementState => _enemyMovementStates;

		protected IDespawnHandler DespawnHandler => _despawnHandler;

		public bool IsPaused { get; protected set; } = false;

		UniqueId IBaseEventReceiver.Id => new();

		private void Awake()
		{
			OnStateMachineAwake();

			_despawnHandler = new DespawnHandler<EnemyStateMachine>(this, _delayBeforeDepsawn);

			_stateMachine.StateChanged += OnStateChanged;

			OnStateMachineBuild();
		}

		private void Update()
		{
			if (IsPaused)
				return;

			_stateMachine.Run();
		}

		private void FixedUpdate()
		{
			if (IsPaused)
				return;

			_stateMachine.FixedRun();
		}

		private void OnDestroy()
		{
			_stateMachine.StateChanged -= OnStateChanged;

			OnStateMachineDestroyed();
		}

		void IPauseable.SetPause(bool isPaused)
		{
			OnPauseStateChanged(isPaused);
		}

		void ISpawnable.OnSpawn()
		{
			OnEnemyAlive();
		}

		void IDespawnable.OnDespawn()
		{
			OnEnemyDespawn();
		}

		void IDamageable.TakeDamage(float damage)
		{
			_health.TakeDamage(damage);

			OnEnemyDamaged();
		}

		void IEventReceiver<PlayerDiedSignal>.OnEvent(PlayerDiedSignal @event)
		{
			OnTargetDied();
		}

		private void SetRunSpeed(float speed)
		{
			if (speed <= 0f)
				throw new ArgumentOutOfRangeException(nameof(speed));

			_runSpeed = speed;
		}

		private void SetAttackDistance(float distance)
		{
			if (distance <= 0f)
				throw new ArgumentOutOfRangeException(nameof(distance));

			_maxAttackDistance = distance;
		}

		private void SetEnemyMaxHealth(float maxHealth)
		{
			_health.SetMaxHealth(maxHealth);
		}

		protected abstract void OnStateMachineBuild();

		protected abstract void OnEnemyDied();

		protected abstract void OnEnemyAlive();

		protected virtual void OnEnemyDespawn() { }

		protected virtual void OnPauseStateChanged(bool isPaused) { }

		protected virtual void OnTargetDied() { }

		protected virtual void OnStateMachineDestroyed() { }

		protected virtual void OnStateMachineAwake() { }

		protected virtual void OnEnemyDamaged() { }

		protected virtual void OnStateChanged(State state) { }

		protected virtual void LoadConfig(EnemyConfig enemyConfig)
		{
			SetAttackDistance(enemyConfig.MaxAttackDistance);

			SetRunSpeed(enemyConfig.MovementSpeed);

			SetEnemyMaxHealth(enemyConfig.MaxEnemyHealth);
		}
	}
}