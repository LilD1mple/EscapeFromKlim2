using EFK2.DialogSystem.Interfaces;
using EFK2.DialogSystem.Text;
using UnityEngine;
using Zenject;

namespace EFK2.Installers
{
	public class DialogScenePlayerInstaller : MonoInstaller
	{
		[SerializeField] private DialogScenePlayer _scenePlayer;

		public override void InstallBindings()
		{
			BindScenePlayer();
		}

		private void BindScenePlayer()
		{
			Container.Bind<IDialogScenePlayer>()
				.To<DialogScenePlayer>()
				.FromInstance(_scenePlayer)
				.AsSingle();
		}
	}
}