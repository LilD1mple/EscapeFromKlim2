using DG.Tweening;
using EFK2.Game.PauseSystem;
using UnityEngine;
using Zenject;

namespace EFK2.UI.View
{
	public class PauseView : MonoBehaviour, IPauseable
	{
		[SerializeField] private CanvasGroup _pauseCanvasGroup;
		[SerializeField] private float _duration;

		[Inject] private readonly PauseService _pauseService;

		private Tween _currentTween;

		private void Awake()
		{
			_pauseService.Register(this);
		}

		private void OnDestroy()
		{
			_pauseService.Unregister(this);
		}

		void IPauseable.SetPause(bool isPaused)
		{
			if (isPaused)
				_pauseCanvasGroup.gameObject.SetActive(true);

			if (_currentTween.IsActive())
				_currentTween.Kill();

			_currentTween = _pauseCanvasGroup.DOFade(isPaused ? 1f : 0f, _duration)
				.OnComplete(() =>
			{
				if (isPaused == false)
					_pauseCanvasGroup.gameObject.SetActive(false);
			})
				.OnKill(() => _currentTween = null)
				.SetRecyclable(true);
		}
	}
}