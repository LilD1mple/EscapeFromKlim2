using EFK2.AI.Interfaces;
using NTC.ContextStateMachine;
using Pathfinding;
using System;
using UnityEngine;

namespace EFK2.AI.States
{
	public sealed class DeathState : State
	{
		private readonly int _deathAnimationTriggerHash = Animator.StringToHash("Die");

		private readonly Action _warriorDead;

		private readonly INavigationAnimatorService _navigationAnimator;

		private readonly AIPath _navigationAgent;

		public DeathState(INavigationAnimatorService navigationAnimator, AIPath navigationAgent, Action action)
		{
			_navigationAnimator = navigationAnimator;

			_navigationAgent = navigationAgent;

			_warriorDead = action;
		}

		public override void OnEnter()
		{
			_navigationAgent.enabled = false;

			_navigationAnimator.SetTrigger(_deathAnimationTriggerHash);

			_warriorDead?.Invoke();
		}
	}
}
