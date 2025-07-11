using EFK2.AI.Interfaces;
using EFK2.Factory;
using EFK2.WeaponSystem.Projectiles;
using NTC.ContextStateMachine;
using Pathfinding;
using UnityEngine;

namespace EFK2.AI.States.Supermagician
{
	public sealed class SpecialAttackState : State
	{
		private readonly float _damage;

		private readonly int _specialAttackTriggerHash = Animator.StringToHash("SpecialAttack");

		private readonly AIPath _aIPath;
		private readonly WizardProjectileFactory _wizardProjectileFactory;
		private readonly Projectile _specialProjectile;

		private readonly INavigationAnimatorService _navigationAnimatorService;

		public SpecialAttackState(AIPath aIPath, WizardProjectileFactory wizardProjectileFactory, Projectile specialProjectile, INavigationAnimatorService navigationAnimatorService, float damage)
		{
			_aIPath = aIPath;
			_wizardProjectileFactory = wizardProjectileFactory;
			_specialProjectile = specialProjectile;
			_navigationAnimatorService = navigationAnimatorService;
			_damage = damage;
		}

		public override void OnEnter()
		{
			_aIPath.enabled = false;

			_wizardProjectileFactory.SetProjectile(_specialProjectile);

			_wizardProjectileFactory.SetProjectileDamage(_damage);

			_navigationAnimatorService.SetTrigger(_specialAttackTriggerHash);
		}
	}
}
