using EFK2.Game.StartSystem;
using EFK2.Player;
using UnityEngine;
using Zenject;

namespace EFK2.Environment
{
	[RequireComponent(typeof(Collider))]
	public class LevelBootstrap : MonoBehaviour
	{
		[Inject] private readonly StartableService _startableService;

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out Unit unit))
			{
				_startableService.StartGame();

				Destroy(gameObject);
			}
		}
	}
}