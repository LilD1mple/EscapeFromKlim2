using Cysharp.Threading.Tasks;
using EFK2.DialogSystem.Interfaces;
using EFK2.DialogSystem.Scenes;
using System;
using TMPro;

namespace EFK2.DialogSystem.Text
{
	public sealed class DialogTextAnimatorService : IDialogTextAnimatorService
	{
		private readonly TMP_Text _personNameText;
		private readonly TMP_Text _personPhraseText;

		private readonly float _writeDelay;

		private bool _isPlaying = false;
		private bool _isPaused = false;

		public bool IsPlaying => _isPlaying;

		public DialogTextAnimatorService(TMP_Text personNameText, TMP_Text personPhraseText, float writeDelay)
		{
			_personNameText = personNameText;
			_personPhraseText = personPhraseText;
			_writeDelay = writeDelay;
		}

		public void AnimateSentence(ref Sentence sentence)
		{
			FillSentence(ref sentence);

			TypeText(sentence.text).Forget();
		}

		public void ClearText()
		{
			_personNameText.text = string.Empty;

			_personPhraseText.text = string.Empty;
		}

		private void FillSentence(ref Sentence sentence)
		{
			_personNameText.text = sentence.speaker.SpeakerName;

			_personNameText.color = sentence.speaker.TextColor;
		}

		private async UniTaskVoid TypeText(string text)
		{
			_personPhraseText.text = string.Empty;

			_isPlaying = true;

			int wordIndex = 0;

			while (_isPlaying)
			{
				if (_isPaused == false)
				{
					_personPhraseText.text += text[wordIndex];

					await UniTask.Delay(TimeSpan.FromSeconds(_writeDelay));

					if (++wordIndex == text.Length)
					{
						_isPlaying = false;

						break;
					}
				}

				await UniTask.Yield(PlayerLoopTiming.Update);
			}
		}

		public void SetPause(bool isPaused)
		{
			_isPaused = isPaused;
		}
	}
}