using EFK2.DialogSystem.Interfaces;
using EFK2.Game.PauseSystem;
using EFK2.Inputs.Interfaces;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace EFK2.DialogSystem.Scenes
{
	public class DialogScenesSequence : MonoBehaviour, IPauseable
	{
		[Header("Story")]
		[SerializeField] private DialogScene _startStoryScene;

		[Header("After Story")]
		[SerializeField] private UnityEvent _storyEnd;

		private DialogScene _currentStoryScene;

		private bool _started = false;
		private bool _isPaused;

		public bool StoryFinished { get; set; }

		private IKeyboardInputService _keyboardInputService;
		private IDialogScenePlayer _storyScenePlayer;

		private PauseService _pauseService;

		private void OnEnable()
		{
			_pauseService.Register(this);
		}

		private void OnDisable()
		{
			_pauseService.Unregister(this);
		}

		private void Update()
		{
			if (_started == false || _isPaused)
				return;

			if (_keyboardInputService.GetPressedKeyDown(KeyCode.Space) || _keyboardInputService.GetPressedKeyDown(KeyCode.Mouse0))
				PlayScene();
		}

		[Inject]
		public void Construct(IKeyboardInputService keyboardInputService, IDialogScenePlayer storyScenePlayer, PauseService pauseService)
		{
			_keyboardInputService = keyboardInputService;

			_storyScenePlayer = storyScenePlayer;

			_pauseService = pauseService;
		}

		public void StartPlay()
		{
			_started = true;

			_currentStoryScene = _startStoryScene;

			_storyScenePlayer.SetDialogScene(_currentStoryScene);

			PlayScene();
		}

		public void SetPause(bool isPaused)
		{
			_isPaused = isPaused;
		}

		public void PlayScene()
		{
			if (_storyScenePlayer.IsPlaying || StoryFinished)
				return;

			if (_storyScenePlayer.IsLastSentence)
			{
				if (_currentStoryScene.NextStoryScene == null)
				{
					_storyEnd?.Invoke();

					return;
				}

				_currentStoryScene = _currentStoryScene.NextStoryScene;

				_storyScenePlayer.ResetSentenceIndex();

				_storyScenePlayer.PlayScene(_currentStoryScene);

				return;
			}

			_storyScenePlayer.PlayNextSentence();
		}
	}
}