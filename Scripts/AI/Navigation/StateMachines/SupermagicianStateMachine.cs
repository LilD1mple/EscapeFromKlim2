using EFK2.AI.Interfaces;
using EFK2.AI.States.Supermagician;
using EFK2.Difficult;
using EFK2.Difficult.Configurations;
using EFK2.Events;
using EFK2.Events.Signals;
using EFK2.Extensions;
using EFK2.Factory;
using EFK2.Game.PauseSystem;
using EFK2.Target.Interfaces;
using EFK2.WeaponSystem.Projectiles;
using NTC.ContextStateMachine;
using Pathfinding;
using UnityEngine;
using Zenject;

namespace EFK2.AI.StateMachines
{
	public class SupermagicianStateMachine : EnemyStateMachine
	{
		[Header("ProjectileFactory")]
		[SerializeField] private WizardProjectileFactory _wizardProjectileFactory;

		[Header("Objects")]
		[SerializeField] private GameObject _evilAura;

		[Header("Projectiles")]
		[SerializeField] private Projectile _defaultProjectile;
		[SerializeField] private Projectile _specialProjectile;

		[Header("Constants")]
		[SerializeField] private float _idleStateExecuteTime;
		[SerializeField] private float _attackStateExecuteTime;
		[SerializeField] private float _specialAttackStateExecuteTime;
		[SerializeField] private float _healStateExecuteTime;

		private float _damage;
		private float _specialDamage;

		private IStateMachineStateSwitcher _stateSwitcher;

		private EventBus _eventBus;
		private PauseService _pauseService;
		private Transform _target;

		private CapsuleCollider _capsuleCollider;
		private AIPath _navigationAgent;

		private INavigationAnimatorService _navigationAnimatorController;

		private AttackState _attackState;
		private SpecialAttackState _specialAttackState;
		private HealState _healState;
		private IdleState _idleState;
		private ChaseState _chaseState;

		private States.DeathState _deathState;

		private readonly int _movementAnimationHash = Animator.StringToHash("Movement");

		[Inject]
		public void Construct(EventBus eventBus, PauseService pauseService, ITargetService playerTarget, IDifficultService difficultSerivce)
		{
			_target = playerTarget.Player;

			_eventBus = eventBus;

			_pauseService = pauseService;

			LoadConfig(difficultSerivce.DifficultConfiguration.SupermagicianWizardConfig);
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
		}

		protected override void OnEnemyAlive()
		{
			_evilAura.EnableObject();

			enabled = true;

			_capsuleCollider.enabled = true;

			_navigationAnimatorController.PlayAnimation(_movementAnimationHash);

			Health.Ressurect();

			_stateSwitcher.Start();
		}

		protected override void OnEnemyDespawn()
		{
			_eventBus.Raise(new EnemyDiedSignal(this));

			enabled = false;
		}

		protected override void OnPauseStateChanged(bool isPaused)
		{
			IsPaused = isPaused;

			isPaused.CompareByTernaryOperation(_stateSwitcher.Stop, _stateSwitcher.Start);

			_navigationAgent.isStopped = isPaused;
		}

		protected override void OnTargetDied()
		{
			_stateSwitcher.Stop();

			StateMachine.SetState(_idleState);
		}

		protected override void OnEnemyDied()
		{
			_stateSwitcher.Stop();

			_capsuleCollider.enabled = false;

			_evilAura.DisableObject();

			DespawnHandler.Despawn();
		}

		protected override void OnStateMachineDestroyed()
		{
			NavigationAnimator.DisableAnimator();

			_eventBus.Unsubscribe(this);

			_pauseService.Unregister(this);
		}

		private void LoadConfig(BossEnemyConfig bossEnemyConfig)
		{
			base.LoadConfig(bossEnemyConfig);

			_damage = bossEnemyConfig.Damage;

			_specialDamage = bossEnemyConfig.SpecialAttackDamage;
		}

		private void GetComponents()
		{
			_stateSwitcher = new StateMachineStateSwitcher(StateMachine, this);

			_navigationAgent = GetComponent<AIPath>();

			_capsuleCollider = GetComponent<CapsuleCollider>();

			_navigationAnimatorController = GetComponent<INavigationAnimatorService>();
		}

		private void InstallStates()
		{
			_idleState = new(_navigationAnimatorController, _navigationAgent);

			_attackState = new(_navigationAnimatorController, _navigationAgent, _wizardProjectileFactory, _defaultProjectile, _damage);

			_specialAttackState = new(_navigationAgent, _wizardProjectileFactory, _specialProjectile, _navigationAnimatorController, _specialDamage);

			_healState = new(_navigationAnimatorController, _navigationAgent);

			_chaseState = new(_target, _navigationAgent, _navigationAnimatorController, Enums.EnemyMovementFlags.Walk, RunSpeed);

			_deathState = new(_navigationAnimatorController, _navigationAgent, OnEnemyDied);
		}

		private void BindTransitions()
		{
			_stateSwitcher
				.AppendState(_idleState, _idleStateExecuteTime)
				.AppendState(_chaseState, () => _navigationAgent.transform.CheckMaxDistanceBetweenTwoTransforms(_target, MaxAttackDistance))
				.AppendState(_attackState, _attackStateExecuteTime)
				.AppendState(_idleState, _idleStateExecuteTime)
				.AppendState(_chaseState, () => _navigationAgent.transform.CheckMaxDistanceBetweenTwoTransforms(_target, MaxAttackDistance))
				.AppendState(_specialAttackState, _specialAttackStateExecuteTime)
				.AppendState(_healState, _healStateExecuteTime);
		}

		private void BindAnyTransitions()
		{
			StateMachine.AddAnyTransition(_deathState, () => Health.IsDead);
		}
	}
}
