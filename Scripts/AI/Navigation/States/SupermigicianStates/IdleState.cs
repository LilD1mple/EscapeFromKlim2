using EFK2.AI.Interfaces;
using NTC.ContextStateMachine;
using Pathfinding;
using UnityEngine;

namespace EFK2.AI.States.Supermagician
{
	public sealed class IdleState : State
	{
		private readonly int _idleAnimationBoolHash = Animator.StringToHash("IsMove");
		private readonly int _idleAnimationHash = Animator.StringToHash("Idle");

		private readonly AIPath _aIPath;

		private readonly INavigationAnimatorService _navigationAnimatorService;

		public IdleState(INavigationAnimatorService navigationAnimatorService, AIPath aIPath)
		{
			_navigationAnimatorService = navigationAnimatorService;
			_aIPath = aIPath;
		}

		public override void OnEnter()
		{
			_aIPath.enabled = false;

			_navigationAnimatorService.SetBool(false, _idleAnimationBoolHash);

			_navigationAnimatorService.PlayAnimation(_idleAnimationHash);
		}

		public override void OnExit()
		{
			_navigationAnimatorService.SetBool(true, _idleAnimationBoolHash);
		}
	}
}
