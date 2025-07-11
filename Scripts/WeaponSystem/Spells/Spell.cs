using EFK2.Extensions;
using EFK2.Game.PauseSystem;
using EFK2.Handlers;
using EFK2.Handlers.Interfaces;
using EFK2.Target.Interfaces;
using NaughtyAttributes;
using NTC.OverlapSugar;
using NTC.Pool;
using System;
using UnityEngine;
using Zenject;
using IPoolable = NTC.Pool.IPoolable;

namespace EFK2.WeaponSystem.Spells
{
	[SelectionBase]
	public abstract class Spell : MonoBehaviour, IPauseable, IPoolable
	{
		[Header("Main")]
		[SerializeField] private OverlapSettings _overlapSettings;

		[Header("Source")]
		[SerializeField] private ParticleSystem _selfParticleVXF;

		[Header("Attack Options")]
		[SerializeField] private bool _startAttackOnSpawn = true;
		[SerializeField, Min(0f)] private float _attackDelay;
		[SerializeField, Min(0f)] private float _damageAmount;

		[Header("Auto Dispose")]
		[SerializeField] private bool _enableAutoDispose = true;
		[SerializeField, ShowIf(nameof(_enableAutoDispose)), Min(0.01f)] private float _autoDisposeTime;

		[Header("Shake Camera")]
		[SerializeField] private bool _enableShake = true;
		[SerializeField, ShowIf(nameof(_enableShake)), Min(0f)] private float _shakeDelay;
		[SerializeField, ShowIf(nameof(_enableShake))] private Vector2 _cameraShakeStrength;

		private bool _isPaused;
		private bool _isDisposed;

		private Camera _shakeCamera;

		private IDespawnHandler _despawnHandler;

		protected IDespawnHandler DespawnHandler => _despawnHandler;

		protected OverlapSettings OverlapSettings => _overlapSettings;

		public ParticleSystem SelfParticleVXF => _selfParticleVXF;

		protected bool IsPaused => _isPaused;

		protected bool IsDisposed => _isDisposed;

		protected float Damage => _damageAmount;

		protected float AttackDelay => _attackDelay;

#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			_overlapSettings.TryDrawGizmos();
		}
#endif

		private void OnParticleCollision(GameObject other)
		{
			OnParticleCollided(other);
		}

		[Inject]
		public void Construct(ITargetService playerTarget)
		{
			_shakeCamera = playerTarget.PlayerController.MainCamera;

			_despawnHandler = new DespawnHandler<Spell>(this, _autoDisposeTime);
		}

		public void SetDamage(float damage)
		{
			if (damage < 0f)
				throw new ArgumentOutOfRangeException(nameof(damage));

			_damageAmount = damage;
		}

		public void ShakeCamera()
		{
			if (_enableShake == false)
				return;

			_shakeCamera.ShakeCamera(_cameraShakeStrength.x, _cameraShakeStrength.y, _shakeDelay);
		}

		void ISpawnable.OnSpawn()
		{
			_isDisposed = false;

			OnSpellSpawn();

			if (_startAttackOnSpawn)
				StartAttack();

			if (_enableAutoDispose)
				_despawnHandler.Despawn();
		}

		void IDespawnable.OnDespawn()
		{
			_isDisposed = true;

			OnSpellDisposed();
		}

		void IPauseable.SetPause(bool isPaused)
		{
			_isPaused = isPaused;

			OnPause(isPaused);
		}

		public virtual void StartAttack() { }

		protected virtual void OnPause(bool isPaused) { }

		protected virtual void OnSpellSpawn() { }

		protected virtual void OnSpellDisposed() { }

		protected virtual void OnPerformAttack() { }

		protected virtual void OnParticleCollided(GameObject other) { }
	}
}