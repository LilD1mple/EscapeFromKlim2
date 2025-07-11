using EFK2.DialogSystem.Actions;
using EFK2.DialogSystem.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace EFK2.Installers
{
	public class SentenceActionsHandlerInstaller : MonoInstaller
	{
		[Header("Source")]
		[SerializeField] private Image _personImage;

		public override void InstallBindings()
		{
			BindSentenceHandler();
		}

		private void BindSentenceHandler()
		{
			Container.Bind<ISentenceActionsHandler>()
				.To<SentenceActionsHandler>()
				.FromNew()
				.AsSingle()
				.WithArguments(_personImage);
		}
	}
}