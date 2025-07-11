using EFK2.AI.Interfaces;
using NTC.ContextStateMachine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace EFK2.AI.States
{
    public sealed class PatrolState : State
    {
        private readonly float _movement;

        private Transform _currentPoint;

        private readonly NavMeshAgent _navMesh;

        private readonly List<Transform> _patrolPoints;

        private readonly INavigationAnimatorService _navigationAnimator;

        private const float _walkSpeed = 2f;

        private readonly int _movementSpeedHash = Animator.StringToHash("MovementSpeed");

        public PatrolState(List<Transform> transforms, NavMeshAgent navMesh, INavigationAnimatorService animatorController, float movement)
        {
            _patrolPoints = transforms;

            _navMesh = navMesh;

            _navigationAnimator = animatorController;

            _movement = movement;
        }

        public override void OnEnter()
        {
            _navMesh.speed = _walkSpeed;

            ChooseRandomPoint();

            _navigationAnimator.SetFloat(_movement, _movementSpeedHash);
        }

        public override void OnRun()
        {
            if (_navMesh.remainingDistance < _navMesh.stoppingDistance)
                ChooseRandomPoint();

            _navMesh.SetDestination(_currentPoint.position);
        }

        public override void OnExit()
        {
            _navMesh.ResetPath();
        }

        private void ChooseRandomPoint()
        {
            Transform newPoint = _patrolPoints[Random.Range(0, _patrolPoints.Count)];

            _currentPoint = newPoint;
        }
    }
}
