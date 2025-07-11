using EFK2.WeaponSystem.Projectiles;
using NTC.Pool;
using UnityEngine;

namespace EFK2.WeaponSystem
{
	public class ProjectileFactory : MonoBehaviour
	{
		[Header("Projectile Template")]
		[SerializeField] private Projectile _crossProjectileTemplate;

		[Header("Spawn Components")]
		[SerializeField] private CrossAnimator _crossAnimator;
		[SerializeField] private Camera _camera;

		[Header("Common")]
		[SerializeField, Min(0f)] private float _force;

		private bool _isPlaying = false;

		public void PerformAttack()
		{
			if (_isPlaying)
				return;

			_isPlaying = true;

			SpawnProjectile();

			_crossAnimator.PlayCrossAttackAnimation(() => _isPlaying = false);
		}

		private void SpawnProjectile()
		{
			Projectile explosiveProjectile = NightPool.Spawn(_crossProjectileTemplate, transform.position, transform.rotation);

			explosiveProjectile.Rigidbody.AddForce(transform.forward * _force, ForceMode.Impulse);
		}
	}
}