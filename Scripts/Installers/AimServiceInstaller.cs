using EFK2.Player.Aim;
using UnityEngine;
using Zenject;

namespace EFK2.Installers
{
	public class AimServiceInstaller : MonoInstaller
	{
		[SerializeField] private AimPresenter _aimPresenter;

		public override void InstallBindings()
		{
			BindAimService();
		}

		private void BindAimService()
		{
			Container
				.Bind<IAimService>()
				.To<AimPresenter>()
				.FromInstance(_aimPresenter)
				.AsSingle();
		}
	}
}