using EFK2.DialogSystem.Audio;
using EFK2.DialogSystem.Interfaces;
using UnityEngine;
using Zenject;

namespace EFK2.Installers
{
	public class DialogAudioServiceInstaller : MonoInstaller
	{
		[Header("Source")]
		[SerializeField] private AudioSource _audioSource;

		public override void InstallBindings()
		{
			BindAudioService();
		}

		private void BindAudioService()
		{
			Container.Bind<IDialogAudioService>()
				.To<DialogAudioService>()
				.FromNew()
				.AsSingle()
				.WithArguments(_audioSource);
		}
	}
}