using Cysharp.Threading.Tasks;
using EFK2.Audio.Interfaces;
using EFK2.Events;
using EFK2.Events.Interfaces;
using EFK2.Events.Signals;
using EFK2.Game.StartSystem;
using System;
using System.Threading;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace EFK2.Audio
{
	public class MusicPlayerPresenter : MonoBehaviour, IStartable, IEventReceiver<PlayerDiedSignal>, IEventReceiver<AllWavesCompleteSignal>
	{
		[Header("Main Music")]
		[SerializeField] private AudioClip[] _backgroundMusics;

		[Header("View")]
		[SerializeField] private MusicPlayerView _musicPlayerView;

		private bool _isRunning = true;

		private int _currentMusicIndex = 0;

		private StartableService _startableService;

		private EventBus _eventBus;

		private CancellationTokenSource _cancellationTokenSource;

		private IMusicService _musicService;

		UniqueId IBaseEventReceiver.Id => new();

		private void Start()
		{
			ShuffleMusic();
		}

		private void OnEnable()
		{
			_eventBus.Subscribe(this as IEventReceiver<AllWavesCompleteSignal>);

			_eventBus.Subscribe(this as IEventReceiver<PlayerDiedSignal>);

			_startableService.Register(this);
		}

		private void OnDisable()
		{
			_eventBus.Unsubscribe(this as IEventReceiver<AllWavesCompleteSignal>);

			_eventBus.Unsubscribe(this as IEventReceiver<PlayerDiedSignal>);

			_startableService.Unregister(this);
		}

		[Inject]
		public void Construct(StartableService startableService, EventBus eventBus, IMusicService musicService)
		{
			_startableService = startableService;

			_musicService = musicService;

			_eventBus = eventBus;
		}

		private async UniTaskVoid PlayMusicLoop()
		{
			_cancellationTokenSource ??= new CancellationTokenSource();

			while (_isRunning)
			{
				AudioClip currentMusic = SelectNextMusic();

				await UniTask.Delay(TimeSpan.FromSeconds(currentMusic.length), cancellationToken: _cancellationTokenSource.Token);
			}
		}

		private AudioClip SelectNextMusic()
		{
			_musicService.ResetMusic(0f);

			AudioClip currentMusic = _backgroundMusics[_currentMusicIndex];

			_currentMusicIndex++;

			if (_currentMusicIndex >= _backgroundMusics.Length - 1)
				_currentMusicIndex = 0;

			_musicService.SetBackgroundMusic(currentMusic);

			_musicPlayerView.ViewMusic(currentMusic);

			return currentMusic;
		}

		private void ShuffleMusic()
		{
			for (int i = _backgroundMusics.Length - 1; i >= 0; i--)
			{
				int j = Random.Range(0, _backgroundMusics.Length - 1);

				(_backgroundMusics[i], _backgroundMusics[j]) = (_backgroundMusics[j], _backgroundMusics[i]);
			}
		}

		void IStartable.StartGame()
		{
			PlayMusicLoop().Forget();
		}

		void IEventReceiver<PlayerDiedSignal>.OnEvent(PlayerDiedSignal @event)
		{
			_isRunning = false;

			_cancellationTokenSource?.Cancel();
			_cancellationTokenSource?.Dispose();

			_cancellationTokenSource = null;

			_musicService.ResetMusic(3f);
		}

		void IEventReceiver<AllWavesCompleteSignal>.OnEvent(AllWavesCompleteSignal @event)
		{
			_musicService.ResetMusic(5f);
		}
	}
}
