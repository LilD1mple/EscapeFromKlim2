using Cysharp.Threading.Tasks;
using DG.Tweening;
using EFK2.Events;
using EFK2.Events.Interfaces;
using EFK2.Events.Signals;
using EFK2.UI.Interfaces;
using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace EFK2.UI
{
	public class TextAnimatorService : MonoBehaviour, IEventReceiver<AllWavesCompleteSignal>, ITextAnimatorService
	{
		[Header("Source")]
		[SerializeField] private TMP_Text _messageText;
		[SerializeField] private Transform _waveStatsTransform;

		[Header("Default Positions")]
		[SerializeField] private Vector3 _otherDefaultPosition;
		[SerializeField] private Vector3 _startMessageTextScale;

		[Header("Settings")]
		[SerializeField] private float _duration;

		private Transform _currentOtherTransform;

		private EventBus _eventBus;

		private const float ShowedMessageTextAlpha = 1f;
		private const float HidedMessageTextAlpha = 0f;
		private const float OtherTransformOffsetByY = 500f;
		private const float ResetAnimatorValuesDuration = 0f;
		private const float TransformOffsetOnAllWavesCompleteByY = 636f;
		private const float TextAnimationInterval = 5f;

		UniqueId IBaseEventReceiver.Id => new();

		[Inject]
		public void Construct(EventBus eventBus)
		{
			_eventBus = eventBus;

			_currentOtherTransform = _waveStatsTransform;
		}

		private void OnEnable()
		{
			_eventBus.Subscribe(this);
		}

		private void OnDisable()
		{
			_eventBus.Unsubscribe(this);
		}

		private void ResetAnimationTransform()
		{
			_messageText.transform.localScale = _startMessageTextScale;

			_messageText.DOFade(HidedMessageTextAlpha, ResetAnimatorValuesDuration);

			_currentOtherTransform.DOLocalMove(_otherDefaultPosition, ResetAnimatorValuesDuration);
		}

		void IEventReceiver<AllWavesCompleteSignal>.OnEvent(AllWavesCompleteSignal @event)
		{
			_currentOtherTransform.DOLocalMoveY(TransformOffsetOnAllWavesCompleteByY, _duration);
		}

		void ITextAnimatorService.AnimateText(string text)
		{
			ResetAnimationTransform();

			_messageText.text = text;

			PlayAnimation().Forget();
		}

		private async UniTaskVoid PlayAnimation()
		{
			await _messageText.DOFade(ShowedMessageTextAlpha, _duration / 2);

			await _messageText.transform.DOScale(Vector3.one, _duration);

			await UniTask.Delay(TimeSpan.FromSeconds(TextAnimationInterval));

			await _messageText.DOFade(HidedMessageTextAlpha, _duration);

			await _currentOtherTransform.DOLocalMoveY(OtherTransformOffsetByY, _duration);
		}
	}
}
