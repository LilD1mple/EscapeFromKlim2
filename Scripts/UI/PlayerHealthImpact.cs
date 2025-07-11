using Cysharp.Threading.Tasks;
using DG.Tweening;
using EFK2.Events;
using EFK2.Events.Interfaces;
using EFK2.Events.Signals;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace EFK2.UI
{
	public class PlayerHealthImpact : MonoBehaviour, IEventReceiver<PlayerHealthChangedSignal>, IEventReceiver<PlayerDiedSignal>
	{
		[Header("Source")]
		[SerializeField] private Image _damageImpactImage;
		[SerializeField] private CanvasGroup _healImpact;

		[Header("Damage Settings")]
		[SerializeField] private float _damageDuration;
		[SerializeField] private Ease _damageEase;

		[Header("Heal Settings")]
		[SerializeField] private float _healDuration;
		[SerializeField] private Ease _healEase;

		[Inject] private readonly EventBus _eventBus;

		private CancellationTokenSource _cancellationTokenSource;

		private const float DamageImageAlphaChannel = 0.05f;
		private const float BrightHealImpactAlphaChannel = 1f;
		private const float HidedHealthImpactAlpha = 0f;
		private const float HidedDamagaImpactAlpha = 0f;

		UniqueId IBaseEventReceiver.Id => new();

		private void OnEnable()
		{
			_eventBus.Subscribe(this as IEventReceiver<PlayerDiedSignal>);

			_eventBus.Subscribe(this as IEventReceiver<PlayerHealthChangedSignal>);
		}

		private void OnDisable()
		{
			_eventBus.Unsubscribe(this as IEventReceiver<PlayerDiedSignal>);

			_eventBus.Unsubscribe(this as IEventReceiver<PlayerHealthChangedSignal>);
		}

		void IEventReceiver<PlayerHealthChangedSignal>.OnEvent(PlayerHealthChangedSignal @event)
		{
			ChooseAnimationByState(@event.Damaged);
		}

		private void ChooseAnimationByState(bool damaged)
		{
			_cancellationTokenSource ??= new CancellationTokenSource();

			if (damaged)
			{
				AnimateDamageImpact().Forget();

				return;
			}

			AnimateHealImpact().Forget();
		}

		private async UniTaskVoid AnimateHealImpact()
		{
			try
			{
				await _healImpact.DOFade(BrightHealImpactAlphaChannel, _healDuration / 2f).SetEase(_healEase).WithCancellation(_cancellationTokenSource.Token);

				await _healImpact.DOFade(HidedHealthImpactAlpha, _healDuration).SetEase(_healEase).WithCancellation(_cancellationTokenSource.Token);
			}
			catch (OperationCanceledException)
			{

			}
			finally
			{
				DisposeToken();
			}
		}

		private async UniTaskVoid AnimateDamageImpact()
		{
			try
			{
				await _damageImpactImage.DOFade(DamageImageAlphaChannel, _damageDuration).SetEase(_damageEase).WithCancellation(_cancellationTokenSource.Token);

				await _damageImpactImage.DOFade(HidedDamagaImpactAlpha, _damageDuration).SetEase(_damageEase).WithCancellation(_cancellationTokenSource.Token);
			}
			catch (OperationCanceledException)
			{

			}
			finally
			{
				DisposeToken();
			}
		}

		private void DisposeToken()
		{
			_cancellationTokenSource?.Dispose();
			_cancellationTokenSource = null;
		}

		void IEventReceiver<PlayerDiedSignal>.OnEvent(PlayerDiedSignal @event)
		{
			_cancellationTokenSource?.Cancel();
		}
	}
}
