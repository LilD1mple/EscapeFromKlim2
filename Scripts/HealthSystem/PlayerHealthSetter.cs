using EFK2.Difficult;
using EFK2.Target.Interfaces;
using UnityEngine;
using Zenject;

namespace EFK2.HealthSystem
{
	public class PlayerHealthSetter : MonoBehaviour
	{
		[SerializeField] private HealthBarView _healthBarView;

		[Inject]
		public void Construct(ITargetService targetService, IDifficultService difficultService)
		{
			float maxHealth = difficultService.DifficultConfiguration.PlayerHealth;

			targetService.Health.SetMaxHealth(maxHealth);

			_healthBarView.Construct(maxHealth);
		}
	}
}