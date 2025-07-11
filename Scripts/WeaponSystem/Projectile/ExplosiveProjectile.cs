using EFK2.Extensions;
using EFK2.Game.PauseSystem;
using EFK2.HealthSystem.Interfaces;
using NTC.OverlapSugar;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace EFK2.WeaponSystem.Projectiles
{
	public class ExplosiveProjectile : Projectile
	{
		[Header("Explosion")]
		[SerializeField] private OverlapSettings _overlapSettings;

		[Inject] private readonly PauseService _pauseService;

		private readonly List<IDamageable> _explosionOverlapResults = new(32);

		private Vector3 _projectileVelocity;
		private Vector3 _projectileAngularVelocity;

#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			_overlapSettings.TryDrawGizmos();
		}
#endif

		protected override void OnProjectileDispose()
		{
			PerformAttack();
		}

		protected override void OnProjectileAwake()
		{
			_pauseService.Register(this);
		}

		protected override void OnProjectileDestroyed()
		{
			_pauseService.Unregister(this);
		}

		protected override void OnPause(bool isPaused)
		{
			if (gameObject.activeSelf)
				isPaused.CompareByTernaryOperation(PauseProjectile, UnpauseProjectile);
		}

		private void PerformAttack()
		{
			if (_overlapSettings.TryFind(_explosionOverlapResults))
				_explosionOverlapResults.ForEach(ApplyDamage);
		}

		private void ApplyDamage(IDamageable damageable)
		{
			damageable.TakeDamage(Damage);
		}

		private void PauseProjectile()
		{
			_projectileVelocity = Rigidbody.velocity;

			_projectileAngularVelocity = Rigidbody.angularVelocity;

			Rigidbody.velocity = Vector3.zero;

			Rigidbody.angularVelocity = Vector3.zero;
		}

		private void UnpauseProjectile()
		{
			Rigidbody.velocity = _projectileVelocity;

			Rigidbody.angularVelocity = _projectileAngularVelocity;
		}
	}
}