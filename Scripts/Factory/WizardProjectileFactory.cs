using EFK2.WeaponSystem.Projectiles;
using NTC.Pool;
using UnityEngine;

namespace EFK2.Factory
{
	public class WizardProjectileFactory : MonoBehaviour
	{
		[Header("Template")]
		[SerializeField] private Projectile _projectileTemplate;

		[Header("Point")]
		[SerializeField] private Transform _point;

		public void SetProjectile(Projectile projectile) => _projectileTemplate = projectile;

		public void SetProjectileDamage(float damage) => _projectileTemplate.SetDamage(damage);

		public void SpawnProjectile() => NightPool.Spawn(_projectileTemplate, _point.position, _point.rotation);
	}
}