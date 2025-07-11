using EFK2.AI.Interfaces;
using EFK2.Enums;
using NTC.ContextStateMachine;
using Pathfinding;
using UnityEngine;

namespace EFK2.AI.States.Supermagician
{
	public sealed class ChaseState : State
	{
		private readonly float _runSpeed;

		private readonly int _movementSpeedHash = Animator.StringToHash("MovementSpeed");
		private readonly int _moveBoolHash = Animator.StringToHash("IsMove");

		private readonly Transform _target;

		private readonly AIPath _navigationAgent;

		private readonly INavigationAnimatorService _navigationAnimatorController;

		private readonly EnemyMovementFlags _movementState;

		public ChaseState(Transform target, AIPath navigationAgent, INavigationAnimatorService navigationAnimatorController, EnemyMovementFlags movementState, float runSpeed)
		{
			_target = target;

			_navigationAgent = navigationAgent;

			_navigationAnimatorController = navigationAnimatorController;

			_movementState = movementState;

			_runSpeed = runSpeed;
		}

		public override void OnEnter()
		{
			_navigationAgent.enabled = true;

			_navigationAgent.maxSpeed = _runSpeed;

			_navigationAnimatorController.SetBool(true, _moveBoolHash);

			_navigationAnimatorController.SetFloat(_movementState == EnemyMovementFlags.Run ? 1f : 0f, _movementSpeedHash);
		}

		public override void OnFixedRun()
		{
			_navigationAgent.destination = _target.position;
		}

		public override void OnExit()
		{
			_navigationAnimatorController.SetBool(false, _moveBoolHash);

			_navigationAgent.enabled = false;
		}
	}
}