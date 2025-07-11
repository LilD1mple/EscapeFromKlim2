using EFK2.Events;
using EFK2.Events.Interfaces;
using EFK2.Events.Signals;
using EFK2.Extensions;
using UnityEngine;
using Zenject;

namespace EFK2.Environment
{
	public class LevelExiter : MonoBehaviour, IEventReceiver<AllWavesCompleteSignal>
	{
		[SerializeField] private GameObject _exitZone;

		[Inject] private readonly EventBus _eventBus;

		UniqueId IBaseEventReceiver.Id => new();

		private void OnEnable()
		{
			_eventBus.Subscribe(this);
		}

		private void OnDisable()
		{
			_eventBus.Unsubscribe(this);
		}

		void IEventReceiver<AllWavesCompleteSignal>.OnEvent(AllWavesCompleteSignal @event)
		{
			_exitZone.EnableObject();
		}
	}
}
