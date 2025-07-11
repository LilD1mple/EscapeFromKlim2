using EFK2.DialogSystem.Interfaces;
using EFK2.DialogSystem.Scenes;
using EFK2.Extensions;
using EFK2.Game.PauseSystem;
using UnityEngine;
using Zenject;

namespace EFK2.DialogSystem.Text
{
	public class DialogScenePlayer : MonoBehaviour, IDialogScenePlayer, IPauseable
	{
		private IDialogTextAnimatorService _textAnimator;
		private IDialogAudioService _dialogAudioService;
		private ISentenceActionsHandler _sentenceActionsHandler;

		private PauseService _pauseService;

		private DialogScene _currentStoryScene;

		private int _sentenceIndex = -1;

		public bool IsPlaying => _textAnimator.IsPlaying;

		public bool IsLastSentence => _sentenceIndex + 1 == _currentStoryScene.Sentences.Count;

		public bool IsFirstSentence => _sentenceIndex == 0;

		private void OnEnable()
		{
			_pauseService.Register(this);
		}

		private void OnDisable()
		{
			_pauseService.Unregister(this);
		}

		[Inject]
		public void Construct(IDialogTextAnimatorService textAnimatorService, IDialogAudioService dialogAudioService, ISentenceActionsHandler sentenceActionsHandler, PauseService pauseService)
		{
			_textAnimator = textAnimatorService;

			_dialogAudioService = dialogAudioService;

			_sentenceActionsHandler = sentenceActionsHandler;

			_pauseService = pauseService;
		}

		public void SetDialogScene(DialogScene dialogScene)
		{
			_currentStoryScene = dialogScene;
		}

		public void PlayScene(DialogScene storyScene)
		{
			_currentStoryScene = storyScene;

			PlaySentence();
		}

		public void PlayNextSentence()
		{
			++_sentenceIndex;

			PlaySentence();
		}

		public void ResetSentenceIndex() => _sentenceIndex = -1;

		private void PlaySentence()
		{
			PrepareSentence();

			Sentence currentSentence = _currentStoryScene.Sentences[_sentenceIndex];

			_sentenceActionsHandler.HandleActions(ref currentSentence);

			_dialogAudioService.PlayClip(currentSentence.sentenceClip);

			_textAnimator.AnimateSentence(ref currentSentence);
		}

		private void PrepareSentence()
		{
			_dialogAudioService.StopClip();

			_textAnimator.ClearText();
		}

		public void SetPause(bool isPaused)
		{
			isPaused.CompareByTernaryOperation(_dialogAudioService.Pause, _dialogAudioService.UnPause);

			_textAnimator.SetPause(isPaused);
		}
	}
}