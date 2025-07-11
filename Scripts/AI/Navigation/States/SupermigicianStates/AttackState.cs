using EFK2.AI.Interfaces;
using EFK2.Factory;
using EFK2.WeaponSystem.Projectiles;
using NTC.ContextStateMachine;
using Pathfinding;
using UnityEngine;

namespace EFK2.AI.States.Supermagician
{
	public sealed class AttackState : State
	{
		private readonly int _attackAnimationHash = Animator.StringToHash("Attack");
		private readonly int _endAttackAnimationTriggerHash = Animator.StringToHash("EndAttack");

		private readonly float _damage;

		private readonly AIPath _aIPath;
		private readonly WizardProjectileFactory _wizardProjectileFactory;
		private readonly Projectile _defaultProjectile;

		private readonly INavigationAnimatorService _navigationAnimatorService;

		public AttackState(INavigationAnimatorService navigationAnimatorService, AIPath aIPath, WizardProjectileFactory wizardProjectileFactory, Projectile defaultProjectile, float damage)
		{
			_navigationAnimatorService = navigationAnimatorService;
			_aIPath = aIPath;
			_wizardProjectileFactory = wizardProjectileFactory;
			_defaultProjectile = defaultProjectile;
			_damage = damage;
		}

		public override void OnEnter()
		{
			_aIPath.enabled = false;

			_wizardProjectileFactory.SetProjectile(_defaultProjectile);

			_wizardProjectileFactory.SetProjectileDamage(_damage);

			_navigationAnimatorService.PlayAnimation(_attackAnimationHash);
		}

		public override void OnExit()
		{
			_navigationAnimatorService.SetTrigger(_endAttackAnimationTriggerHash);
		}
	}
}
