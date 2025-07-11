using EFK2.AI.Interfaces;
using NTC.ContextStateMachine;
using Pathfinding;
using UnityEngine;

namespace EFK2.AI.States.Supermagician
{
	public sealed class HealState : State
	{
		private readonly int _healAnimationTriggerHash = Animator.StringToHash("Buff");

		private readonly AIPath _aIPath;

		private readonly INavigationAnimatorService _navigationAnimatorService;

		public HealState(INavigationAnimatorService navigationAnimatorService, AIPath aIPath)
		{
			_navigationAnimatorService = navigationAnimatorService;
			_aIPath = aIPath;
		}

		public override void OnEnter()
		{
			_aIPath.enabled = false;

			_navigationAnimatorService.SetTrigger(_healAnimationTriggerHash);
		}
	}
}
