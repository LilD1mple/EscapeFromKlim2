using Cysharp.Threading.Tasks;
using EFK2.Difficult;
using EFK2.Events;
using EFK2.Events.Interfaces;
using EFK2.Events.Signals;
using EFK2.Game.PauseSystem;
using EFK2.Game.StartSystem;
using NTC.Pool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using Zenject;

namespace EFK2.HealthSystem
{
	public class HealItemPlacer : MonoBehaviour, IEventReceiver<HealItemsCountChangedSignal>, IEventReceiver<PlayerDiedSignal>, IPauseable, IStartable
	{
		[Header("Prefab")]
		[SerializeField] private HealthItemCollectable _healthItemPrefab;

		[Header("Points")]
		[SerializeField] private List<Transform> _placementPoints;

		[Header("Settings")]
		[SerializeField, Min(0f)] private int _maxHealItemsCount;

		private int _currentHealItemsCount = 0;

		private float _placeInterval;

		private bool _isPaused = false;
		private bool _isRunning = true;

		private EventBus _eventBus;
		private PauseService _pauseService;
		private StartableService _startableService;

		private CancellationTokenSource _cancellationTokenSource;

		private readonly Dictionary<Transform, HealthItemCollectable> _occupiedPoints = new();

		UniqueId IBaseEventReceiver.Id => new();

		private void Awake()
		{
			for (int i = 0; i < _placementPoints.Count; i++)
			{
				_occupiedPoints[_placementPoints[i]] = null;
			}
		}

		private void OnEnable()
		{
			_startableService.Register(this);

			_eventBus.Subscribe(this as IEventReceiver<HealItemsCountChangedSignal>);

			_eventBus.Subscribe(this as IEventReceiver<PlayerDiedSignal>);

			_pauseService.Register(this);
		}

		private void OnDisable()
		{
			_startableService.Unregister(this);

			_eventBus.Unsubscribe(this as IEventReceiver<HealItemsCountChangedSignal>);

			_eventBus.Unsubscribe(this as IEventReceiver<PlayerDiedSignal>);

			_pauseService.Unregister(this);
		}

		[Inject]
		public void Construct(EventBus eventBus, PauseService pauseService, StartableService startableService, IDifficultService difficultSerivce)
		{
			_pauseService = pauseService;

			_eventBus = eventBus;

			_startableService = startableService;

			_placeInterval = difficultSerivce.DifficultConfiguration.HealItemSpawnInterval;

			enabled = difficultSerivce.DifficultConfiguration.EnableSpawnHealItems;
		}

		private async UniTaskVoid SpawnHealLoop()
		{
			_cancellationTokenSource ??= new CancellationTokenSource();

			while (_isRunning)
			{
				await UniTask.Delay(TimeSpan.FromSeconds(_placeInterval), cancellationToken: _cancellationTokenSource.Token).SuppressCancellationThrow();

				if (_currentHealItemsCount < _maxHealItemsCount && _isPaused == false && _cancellationTokenSource.IsCancellationRequested == false)
					PlaceHealItem();
			}
		}

		private void PlaceHealItem()
		{
			Transform currentPoint = FindFreePoint();

			if (currentPoint == null)
				return;

			HealthItemCollectable newHealthItem = NightPool.Spawn(_healthItemPrefab, currentPoint.position, Quaternion.identity);

			_occupiedPoints[currentPoint] = newHealthItem;

			_currentHealItemsCount++;
		}

		private Transform FindFreePoint()
		{
			return _occupiedPoints.Where(e => e.Value == null).Select(p => p.Key).FirstOrDefault();
		}

		void IEventReceiver<HealItemsCountChangedSignal>.OnEvent(HealItemsCountChangedSignal @event)
		{
			Transform placementPoint = _occupiedPoints.First(v => v.Value == @event.HealthItem).Key;

			_occupiedPoints[placementPoint] = null;

			_currentHealItemsCount--;
		}

		void IEventReceiver<PlayerDiedSignal>.OnEvent(PlayerDiedSignal @event)
		{
			_isRunning = false;

			_cancellationTokenSource?.Cancel();
			_cancellationTokenSource?.Dispose();

			_cancellationTokenSource = null;
		}

		void IPauseable.SetPause(bool isPaused)
		{
			_isPaused = isPaused;
		}

		void IStartable.StartGame()
		{
			SpawnHealLoop().Forget();
		}
	}
}