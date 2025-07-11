using Cysharp.Threading.Tasks;
using DG.Tweening;
using EFK2.Events;
using EFK2.Events.Interfaces;
using EFK2.Events.Signals;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace EFK2.HealthSystem
{
	public class HealthBarView : MonoBehaviour, IEventReceiver<PlayerHealthChangedSignal>
	{
		[Header("UI")]
		[SerializeField] private Image _healthBar;
		[SerializeField] private Image _damageAmount;
		[SerializeField] private TMP_Text _healthAmountText;

		[Header("Settings")]
		[SerializeField] private float _duration;

		[Inject] private readonly EventBus _eventBus;

		private const float WaitDelay = 0.3f;

		UniqueId IBaseEventReceiver.Id => new();

		private void OnEnable()
		{
			_eventBus.Subscribe(this);
		}

		private void OnDisable()
		{
			_eventBus.Unsubscribe(this);
		}

		public void Construct(float maxHealth)
		{
			ViewHealth(maxHealth, maxHealth);
		}

		public void OnEvent(PlayerHealthChangedSignal @event)
		{
			float currentHealthRatio = Mathf.InverseLerp(0f, @event.PlayerHeath.MaxHealth, @event.PlayerHeath.CurrentHealth);

			ViewHealth(@event.PlayerHeath.CurrentHealth, @event.PlayerHeath.MaxHealth);

			if (@event.Damaged)
				PlayDamageAnimation(currentHealthRatio).Forget();
			else
				PlayHealAnimation(ref currentHealthRatio);
		}

		private void ViewHealth(float currentHealth, float maxHealth)
		{
			_healthAmountText.text = $"{currentHealth}/{maxHealth}";
		}

		private async UniTaskVoid PlayDamageAnimation(float currentHealthRatio)
		{
			await _healthBar.DOFillAmount(currentHealthRatio, _duration / 2);

			await UniTask.Delay(TimeSpan.FromSeconds(WaitDelay));

			await _damageAmount.DOFillAmount(currentHealthRatio, _duration);
		}

		private void PlayHealAnimation(ref float currentHealthRatio)
		{
			_healthBar.DOFillAmount(currentHealthRatio, _duration);

			_damageAmount.DOFillAmount(currentHealthRatio, _duration);
		}
	}
}