using EFK2.Difficult;
using UnityEngine;
using Zenject;

namespace EFK2.WeaponSystem.Spells
{
	public class SpellsConfigsInstaller : MonoBehaviour
	{
		[SerializeField] private SpellPresenter _lightningSpellPresenter;
		[SerializeField] private SpellPresenter _temporarySpellPresenter;

		[Inject]
		public void Construct(IDifficultService difficultService)
		{
			_lightningSpellPresenter?.LoadConfig(difficultService.DifficultConfiguration.LightningSpellConfig);

			_temporarySpellPresenter?.LoadConfig(difficultService.DifficultConfiguration.TemporacySpellConfig);
		}
	}
}