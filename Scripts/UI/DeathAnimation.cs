using Cysharp.Threading.Tasks;
using DG.Tweening;
using EFK2.Events;
using EFK2.Events.Interfaces;
using EFK2.Events.Signals;
using EFK2.Game.SceneService;
using EFK2.Game.SceneService.Interfaces;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace EFK2.UI
{
	public class DeathAnimation : MonoBehaviour, IEventReceiver<PlayerDiedSignal>
	{
		[Header("Source")]
		[SerializeField] private Image _sourceImage;

		[Header("Common")]
		[SerializeField] private float _animationDuration;

		private EventBus _eventBus;

		private ISceneLoaderService _sceneLoaderService;

		private const float ShowedDeathImageAlpha = 1f;
		private const float WaitDelay = 1f;

		UniqueId IBaseEventReceiver.Id => new();

		private void OnEnable()
		{
			_eventBus.Subscribe(this);
		}

		private void OnDisable()
		{
			_eventBus.Unsubscribe(this);
		}

		[Inject]
		public void Construct(EventBus eventBus, ISceneLoaderService sceneLoaderService)
		{
			_eventBus = eventBus;

			_sceneLoaderService = sceneLoaderService;
		}

		void IEventReceiver<PlayerDiedSignal>.OnEvent(PlayerDiedSignal @event)
		{
			ShowDeathAnimation().Forget();
		}

		private async UniTaskVoid ShowDeathAnimation()
		{
			await _sourceImage.DOFade(ShowedDeathImageAlpha, _animationDuration);

			await UniTask.Delay(TimeSpan.FromSeconds(WaitDelay));

			LoadGameOverScene();
		}

		private void LoadGameOverScene()
		{
			_sceneLoaderService.SetEnablePressKey(false);

			_sceneLoaderService.LoadSceneLegacy(SceneNames.GameOver);
		}
	}
}