using Cysharp.Threading.Tasks;
using EFK2.AI.Interfaces;
using NTC.ContextStateMachine;
using System;
using UnityEngine;


namespace EFK2.AI.States
{
	public sealed class AwakeState : State
	{
		private readonly INavigationAnimatorService _navigationAnimatorService;

		private readonly float _animationDuration;

		private readonly int _awakeAnimationTriggerHash = Animator.StringToHash("Awake");

		public AwakeState(INavigationAnimatorService navigationAnimatorService, float animationDuration)
		{
			_navigationAnimatorService = navigationAnimatorService;
			_animationDuration = animationDuration;
		}

		public bool IsComplete { get; private set; } = false;

		public override void OnEnter()
		{
			IsComplete = false;

			_navigationAnimatorService.SetTrigger(_awakeAnimationTriggerHash);

			WaitForAnimationEnd().Forget();
		}

		public override void OnExit()
		{
			IsComplete = false;
		}

		private async UniTaskVoid WaitForAnimationEnd()
		{
			await UniTask.Delay(TimeSpan.FromSeconds(_animationDuration));

			IsComplete = true;
		}
	}
}