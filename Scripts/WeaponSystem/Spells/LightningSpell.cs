using Cysharp.Threading.Tasks;
using DG.Tweening;
using EFK2.HealthSystem.Interfaces;
using NTC.OverlapSugar;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EFK2.WeaponSystem.Spells
{
	public class LightningSpell : Spell
	{
		[Header("Effects")]
		[SerializeField] private ParticleSystem[] _particleSystems;

		[Header("Lightning")]
		[SerializeField] private Light _light;
		[SerializeField] private float _lightningDelay;
		[SerializeField] private float _lightIntensity;
		[SerializeField] private float _lightDuration;

		private readonly List<IDamageable> _overlapResults = new(32);

		private const float FlashLightDuration = 0f;
		private const float DefaultLightIntensity = 0f;

		public override void StartAttack()
		{
			for (int i = 0; i < _particleSystems.Length - 1; i++)
			{
				_particleSystems[i].Play();
			}

			WaitForDelay().Forget();
		}

		protected override void OnPerformAttack()
		{
			if (OverlapSettings.TryFind(_overlapResults))
				_overlapResults.ForEach(ApplyDamage);
		}

		protected override void OnSpellSpawn()
		{
			PlayFlashLightAnimation().Forget();
		}

		private void ApplyDamage(IDamageable damageable)
		{
			damageable.TakeDamage(Damage);
		}

		private async UniTaskVoid PlayFlashLightAnimation()
		{
			await UniTask.Delay(TimeSpan.FromSeconds(_lightningDelay));

			await _light.DOIntensity(_lightIntensity, FlashLightDuration);

			await _light.DOIntensity(DefaultLightIntensity, _lightDuration);
		}

		private async UniTaskVoid WaitForDelay()
		{
			if (AttackDelay > 0)
			{
				await UniTask.Delay(TimeSpan.FromSeconds(AttackDelay));
			}

			OnPerformAttack();

			ShakeCamera();
		}
	}
}