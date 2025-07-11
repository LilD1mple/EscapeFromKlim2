using EFK2.Game.Save;
using Zenject;

namespace EFK2.Installers
{
	public class ProgressServiceInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			BindProgressService();
		}

		private void BindProgressService()
		{
			Container
				.Bind<IProgressService>()
				.To<ProgressService>()
				.FromNew()
				.AsSingle();
		}
	}
}