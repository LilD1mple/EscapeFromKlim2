using EFK2.AI.Interfaces;
using EFK2.AI.States;
using EFK2.Difficult;
using EFK2.Difficult.Configurations;
using EFK2.Events;
using EFK2.Events.Signals;
using EFK2.Extensions;
using EFK2.Game.PauseSystem;
using EFK2.HealthSystem;
using EFK2.Target.Interfaces;
using EFK2.WeaponSystem.PostEffects;
using Pathfinding;
using UnityEngine;
using Zenject;

namespace EFK2.AI.StateMachines
{
	public class FlashKamikazeStateMachine : EnemyStateMachine
	{
		[Header("Explosion")]
		[SerializeField] private ProjectilePostEffect _projectileEffectPrefab;
		[SerializeField] private Transform _explosionPoint;
		[SerializeField, Min(1f)] private float _explosionLifeTime = 3f;
		[SerializeField, Min(0.01f)] private float _explosionRadius;

		private float _damage;

		private EventBus _eventBus;
		private PauseService _pauseService;
		private Transform _target;

		private Health _playerHealth;

		private CapsuleCollider _capsuleCollider;

		private AIPath _navigationAgent;

		private INavigationAnimatorService _navigationAnimatorController;

		private ChaseState _chaseState;
		private SelfDestructionState _destructionState;

		private readonly int _movementAnimationHash = Animator.StringToHash("Movement");

		[Inject]
		public void Construct(IDifficultService difficultService, ITargetService playerTarget, EventBus eventBus, PauseService pauseService)
		{
			_target = playerTarget.Player;

			_playerHealth = playerTarget.Health;

			_eventBus = eventBus;

			_pauseService = pauseService;

			LoadConfig(difficultService.DifficultConfiguration.FlashKamikazeConfig);
		}

		protected override void OnStateMachineAwake()
		{
			NavigationAnimator.EnableAnimator();

			_pauseService.Register(this);

			_eventBus.Subscribe(this);
		}

		protected override void OnStateMachineBuild()
		{
			GetComponents();

			InstallStates();

			BindAnyTransitions();

			StateMachine.SetState(_chaseState);
		}

		protected override void OnEnemyAlive()
		{
			enabled = true;

			_capsuleCollider.enabled = true;

			_navigationAnimatorController.PlayAnimation(_movementAnimationHash);

			Health.Ressurect();

			StateMachine.SetState(_chaseState);
		}

		protected override void OnEnemyDespawn()
		{
			_eventBus.Raise(new EnemyDiedSignal(this));

			enabled = false;
		}

		protected override void OnPauseStateChanged(bool isPaused)
		{
			IsPaused = isPaused;

			_navigationAgent.isStopped = isPaused;
		}

		protected override void OnTargetDied()
		{
			StateMachine.TransitionsEnabled = false;
		}

		protected override void OnEnemyDied()
		{
			_capsuleCollider.enabled = false;

			DespawnHandler.Despawn();
		}

		protected override void OnStateMachineDestroyed()
		{
			NavigationAnimator.DisableAnimator();

			_eventBus.Unsubscribe(this);

			_pauseService.Unregister(this);
		}

		protected override void LoadConfig(EnemyConfig enemyConfig)
		{
			base.LoadConfig(enemyConfig);

			_damage = enemyConfig.Damage;
		}

		private void GetComponents()
		{
			_navigationAgent = GetComponent<AIPath>();

			_capsuleCollider = GetComponent<CapsuleCollider>();

			_navigationAnimatorController = GetComponent<INavigationAnimatorService>();
		}

		private void InstallStates()
		{
			_chaseState = new(_target, _navigationAgent, _navigationAnimatorController, EnemyMovementState, RunSpeed);

			_destructionState = new(_navigationAnimatorController, _projectileEffectPrefab,
				transform, _explosionPoint, _playerHealth, _eventBus, OnEnemyDied, _explosionRadius, _damage, _explosionLifeTime);
		}

		private void BindAnyTransitions()
		{
			StateMachine.AddAnyTransition(_destructionState, () => Health.IsDead || _navigationAgent.transform.CheckMaxDistanceBetweenTwoTransforms(_target, MaxAttackDistance));
		}
	}
}