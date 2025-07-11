using EFK2.Events;
using EFK2.Events.Signals;
using EFK2.Target.Interfaces;
using UnityEngine;
using Zenject;

namespace EFK2.HealthSystem
{
	public class PlayerDeathProvider : MonoBehaviour
	{
		private Health _playerHealth;
		private EventBus _eventBus;

		[Inject]
		public void Construct(ITargetService playerTarget, EventBus eventBus)
		{
			_playerHealth = playerTarget.Health;

			_eventBus = eventBus;
		}

		private void OnEnable()
		{
			_playerHealth.Died += OnPlayerKilled;
		}

		private void OnDisable()
		{
			_playerHealth.Died -= OnPlayerKilled;
		}

		private void OnPlayerKilled()
		{
			_eventBus.Raise(new PlayerDiedSignal());
		}
	}
}