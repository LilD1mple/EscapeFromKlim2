using EFK2.Events;
using EFK2.Extensions;
using EFK2.Game.PauseSystem;
using EFK2.HealthSystem;
using EFK2.Target.Interfaces;
using UnityEngine;
using Zenject;

namespace EFK2.WeaponSystem.Projectiles
{
	public class EnemyProjectile : Projectile
	{
		[Header("Explosion")]
		[SerializeField] private float _explosionRadius;

		[Header("Projectile Movement")]
		[SerializeField] private Vector3 _targetOffset;
		[SerializeField] private float _sideMovementAngle;
		[SerializeField] private float _upMovementAngle;
		[SerializeField] private float _movementSpeed;

		private float _randomSideAngle;
		private float _randomUpAngle;

		private Transform _targetPoint;

		private Health _playerHealth;
		private PauseService _pauseService;
		private EventBus _eventBus;

		private Vector3 _targetPosition;

		private Vector3 _projectileVelocity;
		private Vector3 _projectileAngularVelocity;

		[Inject]
		public void Construct(ITargetService playerTarget, PauseService pauseService, EventBus eventBus)
		{
			_playerHealth = playerTarget.Health;

			_targetPoint = playerTarget.TargetPoint;

			_pauseService = pauseService;

			_eventBus = eventBus;
		}

		protected override void OnProjectileAwake()
		{
			_pauseService.Register(this);

			_randomUpAngle = Random.Range(0, _upMovementAngle);

			_randomSideAngle = Random.Range(-_sideMovementAngle, _sideMovementAngle);
		}

		protected override void OnProjectileSpawn()
		{
			UpdateTargetPosition();
		}

		protected override void OnProjectileDestroyed()
		{
			_pauseService.Unregister(this);
		}

		protected override void OnProjectileRun()
		{
			CalculateMovement();
		}

		protected override void OnProjectileDispose()
		{
			PerformAttack();
		}

		protected override void OnPause(bool isPaused)
		{
			if (gameObject.activeSelf)
				isPaused.CompareByTernaryOperation(PauseProjectile, UnpauseProjectile);
		}

		private void CalculateMovement()
		{
			Vector3 forward = _targetPosition + _targetOffset - transform.position;
			Vector3 crossDirection = Vector3.Cross(forward, Vector3.up);

			Quaternion randomDeltaRotation = Quaternion.Euler(0, _randomSideAngle, 0) * Quaternion.AngleAxis(_randomUpAngle, crossDirection);
			Vector3 direction = randomDeltaRotation * (_targetPosition + _targetOffset - transform.position);

			float distanceThisFrame = Time.deltaTime * _movementSpeed;

			transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, distanceThisFrame);
			transform.rotation = Quaternion.LookRotation(direction);
		}

		private void UpdateTargetPosition()
		{
			_targetPosition = _targetPoint.position;
		}

		private void PerformAttack()
		{
			if (transform.CheckMaxDistanceBetweenTwoTransforms(_playerHealth.transform, _explosionRadius))
				_playerHealth.DamagePlayer(_eventBus, Damage);
		}

		private void PauseProjectile()
		{
			_projectileVelocity = Rigidbody.velocity;

			_projectileAngularVelocity = Rigidbody.angularVelocity;

			Rigidbody.velocity = Vector3.zero;

			Rigidbody.angularVelocity = Vector3.zero;
		}

		private void UnpauseProjectile()
		{
			Rigidbody.velocity = _projectileVelocity;

			Rigidbody.angularVelocity = _projectileAngularVelocity;
		}
	}
}