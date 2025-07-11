using Cysharp.Threading.Tasks;
using DG.Tweening;
using EFK2.Inputs.Interfaces;
using System;
using System.Threading;
using UnityEngine;
using Zenject;

namespace EFK2.Environment
{
	public class SkipButtonPressOrAnimationEndAwaiter : MonoBehaviour
	{
		[Header("Source")]
		[SerializeField] private MainMenuTransition _mainMenuTransition;
		[SerializeField] private CreditsBackgroundMusicPresenter _creditsBackgroundMusicPresenter;
		[SerializeField] private Transform _creditsTextTransform;

		[Header("Key")]
		[SerializeField] private KeyCode _skipKeyCode;

		[Header("Animation Settings")]
		[SerializeField] private Ease _animationEase;
		[SerializeField] private Vector2 _reachPosition;
		[SerializeField] private float _animationDuration;
		[SerializeField] private float _animationDelay;

		[Inject] private readonly IKeyboardInputService _keyboardInputService;

		private Tween _currentAnimation;

		private CancellationTokenSource _cancellationTokenSource;

		private void Start()
		{
			DisposeAndStartAwait();
		}

		private void OnDestroy()
		{
			DisposeTokenAndAnimation();
		}

		private void DisposeAndStartAwait()
		{
			DisposeTokenAndAnimation();

			_cancellationTokenSource ??= new CancellationTokenSource();

			WaitPressKeyOrEndAnimation(_cancellationTokenSource.Token).Forget();
		}

		private void DisposeTokenAndAnimation()
		{
			_cancellationTokenSource?.Cancel();
			_cancellationTokenSource?.Dispose();

			_cancellationTokenSource = null;

			_currentAnimation?.Kill();
			_currentAnimation = null;
		}

		private void StartTransition()
		{
			_creditsBackgroundMusicPresenter.ResetMusic();

			_mainMenuTransition.StartTransition();
		}

		private async UniTaskVoid WaitPressKeyOrEndAnimation(CancellationToken externalToken = default)
		{
			var linkedCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(externalToken, this.GetCancellationTokenOnDestroy());

			CancellationToken token = linkedCancellationTokenSource.Token;

			try
			{
				_currentAnimation = _creditsTextTransform
					.DOLocalMove(_reachPosition, _animationDuration)
					.SetDelay(_animationDelay)
					.SetEase(_animationEase);

				UniTask animationTask = _currentAnimation.WithCancellation(token);

				UniTask keyPressTask = UniTask.WaitUntil(() => _keyboardInputService.GetPressedKeyDown(_skipKeyCode), cancellationToken: token);

				int winnerIndex = await UniTask.WhenAny(animationTask, keyPressTask);

				token.ThrowIfCancellationRequested();

				if (winnerIndex == 1)
				{
					_currentAnimation?.Kill();
					_currentAnimation = null;
				}

				StartTransition();
			}
			catch (OperationCanceledException)
			{
				_currentAnimation?.Kill();
				_currentAnimation = null;
			}
			finally
			{
				linkedCancellationTokenSource?.Dispose();
				_cancellationTokenSource?.Dispose();

				_cancellationTokenSource = null;
				_currentAnimation = null;
			}
		}
	}
}