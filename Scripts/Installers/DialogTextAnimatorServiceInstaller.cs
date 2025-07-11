using EFK2.DialogSystem.Interfaces;
using EFK2.DialogSystem.Text;
using TMPro;
using UnityEngine;
using Zenject;

namespace EFK2.Installers
{
	public class DialogTextAnimatorServiceInstaller : MonoInstaller
	{
		[Header("Source")]
		[SerializeField] private TMP_Text _personNameText;
		[SerializeField] private TMP_Text _personPhraseText;

		[Header("Constants")]
		[SerializeField] private float _writeDelay;

		public override void InstallBindings()
		{
			BindTextAnimator();
		}

		private void BindTextAnimator()
		{
			Container
				.Bind<IDialogTextAnimatorService>()
				.To<DialogTextAnimatorService>()
				.FromNew()
				.AsSingle()
				.WithArguments(_personNameText, _personPhraseText, _writeDelay);
		}
	}
}