using EFK2.Game.StartSystem;
using EFK2.UI.Interfaces;
using UnityEngine;
using Zenject;

namespace EFK2.Environment
{
	public class LevelGuide : MonoBehaviour, IStartable
	{
		[Header("Messages")]
		[SerializeField] private string _startGuideMessage;

		private StartableService _startableService;

		private ITextAnimatorService _textAnimator;

		[Inject]
		public void Construct(StartableService startableService, ITextAnimatorService textAnimator)
		{
			_startableService = startableService;

			_textAnimator = textAnimator;
		}

		private void OnEnable()
		{
			_startableService.Register(this);
		}

		private void OnDisable()
		{
			_startableService.Unregister(this);
		}

		void IStartable.StartGame()
		{
			_textAnimator.AnimateText(_startGuideMessage);
		}
	}
}