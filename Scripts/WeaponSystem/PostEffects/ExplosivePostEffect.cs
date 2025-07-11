using Cysharp.Threading.Tasks;
using DG.Tweening;
using EFK2.Extensions;
using EFK2.Target.Interfaces;
using UnityEngine;
using Zenject;

namespace EFK2.WeaponSystem.PostEffects
{
	public class ExplosivePostEffect : ProjectilePostEffect
	{
		[Header("Effect Components")]
		[SerializeField] private Light _explosiveFlashLight;

		[Header("Shake Settings")]
		[SerializeField] private Vector2 _explosionDuration;

		private Camera _effectCamera;

		private const float BrightFlashValue = 2f;
		private const float BrightFlashAnimationDuration = 0f;
		private const float DefaultLightIntensityValue = 0f;

		[Inject]
		public void Construct(ITargetService playerTarget)
		{
			_effectCamera = playerTarget.PlayerController.MainCamera;
		}

		protected override void OnPostEffectStarted()
		{
			_effectCamera.ShakeCamera(_explosionDuration.x, _explosionDuration.y);

			AnimateLightFlash().Forget();
		}

		private async UniTaskVoid AnimateLightFlash()
		{
			await _explosiveFlashLight.DOIntensity(BrightFlashValue, BrightFlashAnimationDuration);

			await _explosiveFlashLight.DOIntensity(DefaultLightIntensityValue, EffectDuration);
		}
	}
}