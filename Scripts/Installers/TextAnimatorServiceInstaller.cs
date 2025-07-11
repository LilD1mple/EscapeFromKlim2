using EFK2.UI;
using EFK2.UI.Interfaces;
using UnityEngine;
using Zenject;

namespace EFK2.Installers
{
	public class TextAnimatorServiceInstaller : MonoInstaller
	{
		[SerializeField] private TextAnimatorService _textAnimatorService;

		public override void InstallBindings()
		{
			BindTextAnimator();
		}

		private void BindTextAnimator()
		{
			Container
				.Bind<ITextAnimatorService>()
				.To<TextAnimatorService>()
				.FromInstance(_textAnimatorService)
				.AsSingle()
				.Lazy();
		}
	}
}