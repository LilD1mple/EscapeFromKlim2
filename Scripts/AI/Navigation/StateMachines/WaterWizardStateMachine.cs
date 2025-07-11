using EFK2.AI.Interfaces;
using EFK2.AI.States;
using EFK2.Difficult;
using EFK2.Difficult.Configurations;
using EFK2.Events;
using EFK2.Events.Signals;
using EFK2.Extensions;
using EFK2.Factory;
using EFK2.Game.PauseSystem;
using EFK2.HealthSystem;
using EFK2.Target.Interfaces;
using NTC.ContextStateMachine;
using Pathfinding;
using UnityEngine;
using Zenject;

namespace EFK2.AI.StateMachines
{
	[RequireComponent(typeof(WizardProjectileFactory))]
	public class WaterWizardStateMachine : EnemyStateMachine
	{
		[SerializeField] private LayerMask _raycastLayerMask;

		[Header("Shield")]
		[SerializeField] private GameObject _waterShieldObject;

		private EventBus _eventBus;
		private PauseService _pauseService;
		private Transform _target;

		private CapsuleCollider _capsuleCollider;

		private AIPath _navigationAgent;

		private INavigationAnimatorService _navigationAnimatorController;

		private IdleState _idleState;
		private ChaseState _chaseState;
		private AttackState _attackState;
		private DeathState _deathState;

		private readonly int _movementAnimationHash = Animator.StringToHash("Movement");

		[Inject]
		public void Construct(EventBus eventBus, PauseService pauseService, ITargetService playerTarget, IDifficultService difficultSerivce)
		{
			_target = playerTarget.Player;

			_eventBus = eventBus;

			_pauseService = pauseService;

			LoadConfig(difficultSerivce.DifficultConfiguration.WaterWizardConfig);
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

			BindTransitions();

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

			StateMachine.SetState(_idleState);
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

		protected override void OnStateChanged(State state)
		{
			bool enable = state is ChaseState;

			_waterShieldObject.SetActive(enable);

			Health.Invincible = enable;
		}

		protected override void LoadConfig(EnemyConfig enemyConfig)
		{
			base.LoadConfig(enemyConfig);

			WizardProjectileFactory wizardProjectileFactory = GetComponent<WizardProjectileFactory>();

			wizardProjectileFactory.SetProjectileDamage(enemyConfig.Damage);
		}

		private void GetComponents()
		{
			_navigationAgent = GetComponent<AIPath>();

			_capsuleCollider = GetComponent<CapsuleCollider>();

			_navigationAnimatorController = GetComponent<INavigationAnimatorService>();
		}

		private void InstallStates()
		{
			_idleState = new(_navigationAnimatorController, _navigationAgent);

			_chaseState = new(_target, _navigationAgent, _navigationAnimatorController, EnemyMovementState, RunSpeed);

			_attackState = new(_navigationAnimatorController);

			_deathState = new(_navigationAnimatorController, _navigationAgent, OnEnemyDied);
		}

		private void BindTransitions()
		{
			StateMachine.AddTransition(_chaseState, _attackState, () => _navigationAgent.transform.RaycastTarget(_target, MaxAttackDistance, _raycastLayerMask));

			StateMachine.AddTransition(_attackState, _chaseState, () => _navigationAgent.transform.RaycastTarget(_target, MaxAttackDistance, _raycastLayerMask) == false);
		}

		private void BindAnyTransitions()
		{
			StateMachine.AddAnyTransition(_deathState, () => Health.IsDead);
		}
	}
}