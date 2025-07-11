using EFK2.AI.Interfaces;
using NTC.ContextStateMachine;
using UnityEngine;

namespace EFK2.AI.States
{
	public sealed class AttackState : State
	{
		private readonly int _attackAnimationHash = Animator.StringToHash("Attack");

		private readonly INavigationAnimatorService _navigationAnimator;

		public AttackState(INavigationAnimatorService navigationAnimatorController)
		{
			_navigationAnimator = navigationAnimatorController;
		}

		public override void OnEnter()
		{
			_navigationAnimator.SetBool(true, _attackAnimationHash);
		}

		public override void OnExit()
		{
			_navigationAnimator.SetBool(false, _attackAnimationHash);
		}
	}
}
