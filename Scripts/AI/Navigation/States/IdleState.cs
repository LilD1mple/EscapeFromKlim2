using EFK2.AI.Interfaces;
using NTC.ContextStateMachine;
using Pathfinding;
using UnityEngine;

namespace EFK2.AI.States
{
	public sealed class IdleState : State
	{
		private readonly int _idleAnimationTriggerHash = Animator.StringToHash("Idle");

		private readonly INavigationAnimatorService _navigationAnimatorController;

		private readonly AIPath _navigationAgent;

		public IdleState(INavigationAnimatorService navigationAnimatorController, AIPath navigationAgent)
		{
			_navigationAnimatorController = navigationAnimatorController;
			_navigationAgent = navigationAgent;
		}

		public override void OnEnter()
		{
			_navigationAgent.enabled = false;

			_navigationAnimatorController.SetTrigger(_idleAnimationTriggerHash);
		}
	}
}
