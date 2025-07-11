using DG.Tweening;
using EFK2.Game.StartSystem;
using UnityEngine;
using Zenject;

namespace EFK2.UI
{
	public class PlayerHUDMover : MonoBehaviour, IStartable
	{
		[SerializeField] private DOTweenAnimation[] _hudTweens;

		private StartableService _startableService;

		[Inject]
		public void Construct(StartableService startableService)
		{
			_startableService = startableService;
		}

		private void OnEnable()
		{
			_startableService.Register(this);
		}

		private void OnDisable()
		{
			_startableService.Unregister(this);
		}

		public void StartGame()
		{
			for (int i = 0; i < _hudTweens.Length; i++)
			{
				_hudTweens[i].DOPlay();
			}
		}
	}
}