using Cysharp.Threading.Tasks;
using EFK2.Game.PauseSystem;
using EFK2.Handlers;
using EFK2.Handlers.Interfaces;
using EFK2.HealthSystem.Interfaces;
using EFK2.WeaponSystem.PostEffects;
using NTC.Pool;
using System;
using System.Threading;
using UnityEngine;

namespace EFK2.WeaponSystem.Projectiles
{
	[SelectionBase]
	[RequireComponent(typeof(Rigidbody))]
	public abstract class Projectile : MonoBehaviour, IPoolable, IPauseable
	{
		[Header("Common")]
		[SerializeField, Min(0f)] private float _damage = 10f;
		[SerializeField] private ProjectileDisposeType _disposeType = ProjectileDisposeType.OnAnyCollision;

		[Header("Rigidbody")]
		[SerializeField] private Rigidbody _projectileRigidbody;

		[Header("Projectile Lifetime")]
		[SerializeField, Min(1f)] private float _projectileLifetime = 5f;

		[Header("Effect On Destroy")]
		[SerializeField] private bool _spawnEffectOnDestroy = true;
		[SerializeField] private ProjectilePostEffect _effectOnDestroyPrefab;
		[SerializeField, Min(0f)] private float _effectOnDestroyLifetime = 2f;

		private IDespawnHandler _selfDespawnHandler;
		private IDespawnHandler _projectilePostEffectDespawnHandler;

		private UniTaskCompletionSource<Collision> _collisionTaskCompletedSource;

		private CancellationTokenSource _linkedCancellationTokenSource;

		private bool _isWaiting = false;

		public bool IsProjectileDisposed { get; private set; }
		public bool IsPaused { get; private set; } = false;
		public float Damage => _damage;
		public ProjectileDisposeType DisposeType => _disposeType;
		public Rigidbody Rigidbody => _projectileRigidbody;

		private void Awake()
		{
			_selfDespawnHandler = new DespawnHandler<Projectile>(this);

			OnProjectileAwake();
		}

		private void Update()
		{
			if (IsPaused)
				return;

			OnProjectileRun();
		}

		private void OnCollisionEnter(Collision collision)
		{
			if (_isWaiting && _collisionTaskCompletedSource != null && !_collisionTaskCompletedSource.Task.Status.IsCompleted())
			{
				_collisionTaskCompletedSource.TrySetResult(collision);
			}
		}

		private void BeginExplosion(Collision collision)
		{
			if (IsProjectileDisposed)
				return;

			if (collision == null)
			{
				DisposeProjectile();

				return;
			}

			if (collision.gameObject.TryGetComponent(out IDamageable damageable))
			{
				OnTargetCollision(collision, damageable);

				if (_disposeType == ProjectileDisposeType.OnTargetCollision)
				{
					DisposeProjectile();
				}
			}
			else
			{
				OnOtherCollision(collision);
			}

			OnAnyCollision(collision);

			if (_disposeType == ProjectileDisposeType.OnAnyCollision)
			{
				DisposeProjectile();
			}
		}

		private void OnDestroy()
		{
			_linkedCancellationTokenSource?.Cancel();
			_linkedCancellationTokenSource?.Dispose();

			_collisionTaskCompletedSource = null;

			_isWaiting = false;

			OnProjectileDestroyed();
		}

		private void StartWaitingForEvent()
		{
			CancelWaiting();

			_isWaiting = true;

			_linkedCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy());

			_collisionTaskCompletedSource = new UniTaskCompletionSource<Collision>();

			WaitForCollisionOrTimeoutAsync(_linkedCancellationTokenSource.Token).Forget();
		}

		private async UniTaskVoid WaitForCollisionOrTimeoutAsync(CancellationToken token)
		{
			try
			{
				UniTask timerTask = UniTask.Delay(TimeSpan.FromSeconds(_projectileLifetime), cancellationToken: token);

				UniTask<Collision> collisionTask = _collisionTaskCompletedSource.Task;

				int winnerIndex = await UniTask.WhenAny(timerTask, collisionTask);

				if (winnerIndex == 0)
				{
					_collisionTaskCompletedSource?.TrySetCanceled(CancellationToken.None);

					BeginExplosion(null);
				}
				else
				{
					Collision collisionData = collisionTask.GetAwaiter().GetResult();

					BeginExplosion(collisionData);
				}
			}
			catch (OperationCanceledException)
			{
				_collisionTaskCompletedSource?.TrySetCanceled(token);
			}
			finally
			{
				_isWaiting = false;

				_linkedCancellationTokenSource?.Cancel();
				_linkedCancellationTokenSource?.Dispose();
				_linkedCancellationTokenSource = null;

				_collisionTaskCompletedSource = null;
			}
		}

		private void CancelWaiting()
		{
			if (_isWaiting)
			{
				_linkedCancellationTokenSource?.Cancel();
			}
		}

		public void DisposeProjectile()
		{
			OnProjectileDispose();

			SpawnEffectOnDestroy();

			_selfDespawnHandler.Despawn();
		}

		public void SetDamage(float damage)
		{
			if (damage < 0)
				throw new ArgumentOutOfRangeException(nameof(damage));

			_damage = damage;
		}

		private void SpawnEffectOnDestroy()
		{
			if (_spawnEffectOnDestroy == false)
				return;

			var effect = NightPool.Spawn(_effectOnDestroyPrefab, transform.position, _effectOnDestroyPrefab.transform.rotation);

			_projectilePostEffectDespawnHandler = new DespawnHandler<ProjectilePostEffect>(effect, _effectOnDestroyLifetime);

			_projectilePostEffectDespawnHandler.Despawn();
		}

		private void ResetRigidbody()
		{
			_projectileRigidbody.velocity = Vector3.zero;

			_projectileRigidbody.angularVelocity = Vector3.zero;
		}

		private void ResetProjectile()
		{
			IsProjectileDisposed = true;

			ResetRigidbody();
		}

		void ISpawnable.OnSpawn()
		{
			IsProjectileDisposed = false;

			StartWaitingForEvent();

			OnProjectileSpawn();
		}

		void IDespawnable.OnDespawn()
		{
			ResetProjectile();
		}

		void IPauseable.SetPause(bool isPaused)
		{
			IsPaused = isPaused;

			OnPause(isPaused);
		}

		protected virtual void OnProjectileAwake() { }
		protected virtual void OnProjectileSpawn() { }
		protected virtual void OnProjectileDestroyed() { }
		protected virtual void OnProjectileDispose() { }
		protected virtual void OnProjectileRun() { }
		protected virtual void OnPause(bool isPaused) { }
		protected virtual void OnAnyCollision(Collision collision) { }
		protected virtual void OnOtherCollision(Collision collision) { }
		protected virtual void OnTargetCollision(Collision collision, IDamageable damageable) { }
	}
}
