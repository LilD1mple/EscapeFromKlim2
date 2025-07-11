using Cysharp.Threading.Tasks;
using EFK2.AI.StateMachines;
using EFK2.Difficult;
using EFK2.Environment;
using EFK2.Events;
using EFK2.Events.Interfaces;
using EFK2.Events.Signals;
using EFK2.Extensions;
using EFK2.Game.PauseSystem;
using EFK2.Game.StartSystem;
using EFK2.UI.Interfaces;
using NTC.Pool;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace EFK2.Factory
{
	public class EnemyFactory : MonoBehaviour, IPauseable, IStartable, IEventReceiver<EnemyDiedSignal>
	{
		[Header("Spawn Points")]
		[SerializeField] private SpawnPoint[] _spawnPoints;

		[Header("Waves")]
		[SerializeField] private List<AttackWave> _attackWaves;

		private float _spawnDelay;

		private int _currentWaveIndex = -1;
		private int _remainingEnemies;
		private int _totalEnemies;

		private bool _isPaused = false;

		private readonly Dictionary<EnemyStateMachine, int> _spawnedEnemies = new();

		private AttackWave _currentWave;

		private PauseService _pauseService;
		private StartableService _startableService;
		private EventBus _eventBus;

		private ITextAnimatorService _textAnimatorService;

		UniqueId IBaseEventReceiver.Id => new();

		public event Action<int> WaveChanged;
		public event Action<int> EnemiesCountChanged;

		private const string NewWaveText = "Новая волна!";

		private void Awake()
		{
			CalculateTotalEnemies(_attackWaves[0]);
		}

		private void OnEnable()
		{
			_startableService.Register(this);

			_pauseService.Register(this);

			_eventBus.Subscribe(this);
		}

		private void OnDisable()
		{
			_startableService.Unregister(this);

			_pauseService.Unregister(this);

			_eventBus.Unsubscribe(this);
		}

		[Inject]
		public void Construct(StartableService startableService, PauseService pauseService, EventBus eventBus, IDifficultService difficultService, ITextAnimatorService textAnimator)
		{
			_pauseService = pauseService;

			_startableService = startableService;

			_eventBus = eventBus;

			_textAnimatorService = textAnimator;

			_spawnDelay = difficultService.DifficultConfiguration.EnemySpawnDelay;
		}

		private async UniTaskVoid SpawnEnemies()
		{
			int spawnedEnemies = 0;

			while (spawnedEnemies != _totalEnemies)
			{
				if (_isPaused == false)
				{
					SpawnEnemy();

					spawnedEnemies++;

					await UniTask.Delay(TimeSpan.FromSeconds(_spawnDelay));
				}

				await UniTask.Yield(PlayerLoopTiming.Update);
			}
		}

		private void SpawnEnemy()
		{
			SpawnPoint currentSpawnPoint = _spawnPoints.PickRandomElementInCollection();

			EnemyType currentEnemy;

			while (true)
			{
				currentEnemy = _currentWave.EnemyOnWaves.PickRandomElementInCollection();

				if (_spawnedEnemies.ContainsKey(currentEnemy.enemy) == false)
					_spawnedEnemies.Add(currentEnemy.enemy, 0);

				if (_spawnedEnemies[currentEnemy.enemy] < currentEnemy.enemiesCountShouldBeSpawned)
					break;
			}

			NightPool.Spawn(currentEnemy.enemy, currentSpawnPoint.transform.position, Quaternion.identity);

			currentSpawnPoint.OnObjectSpawned();

			_spawnedEnemies[currentEnemy.enemy]++;
		}

		private void SetupNewWave()
		{
			_currentWaveIndex++;

			if (_currentWaveIndex >= _attackWaves.Count)
			{
				_eventBus.Raise(new AllWavesCompleteSignal());

				return;
			}

			_spawnedEnemies.Clear();

			_currentWave = _attackWaves[_currentWaveIndex];

			WaveChanged?.Invoke(_currentWaveIndex + 1);

			if (_currentWaveIndex != 0)
				_textAnimatorService.AnimateText(NewWaveText);

			CalculateTotalEnemies(_currentWave);

			SpawnEnemies().Forget();
		}

		private void CalculateTotalEnemies(AttackWave currentWave)
		{
			_remainingEnemies = 0;

			_totalEnemies = 0;

			for (int i = 0; i < currentWave.EnemyOnWaves.Count; i++)
			{
				_remainingEnemies += currentWave.EnemyOnWaves[i].enemiesCountShouldBeSpawned;

				_totalEnemies += currentWave.EnemyOnWaves[i].enemiesCountShouldBeSpawned;
			}
		}

		void IPauseable.SetPause(bool isPaused)
		{
			_isPaused = isPaused;
		}

		void IStartable.StartGame()
		{
			SetupNewWave();

			EnemiesCountChanged?.Invoke(_remainingEnemies);
		}

		void IEventReceiver<EnemyDiedSignal>.OnEvent(EnemyDiedSignal @event)
		{
			_remainingEnemies--;

			if (_remainingEnemies == 0)
				SetupNewWave();

			EnemiesCountChanged?.Invoke(_remainingEnemies);
		}
	}
}