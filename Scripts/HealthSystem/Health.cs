using EFK2.HealthSystem.Interfaces;
using NaughtyAttributes;
using System;
using UnityEngine;

namespace EFK2.HealthSystem
{
	public class Health : MonoBehaviour, IHealable
	{
		[Header("Health Settings")]
		[SerializeField, Min(0f)] private float _maxHealth = 10f;
		[field: SerializeField] public bool Invincible { get; set; } = false;

		[Header("Armor")]
		[SerializeField] private bool _enableArmor = false;
		[SerializeField, Min(0f), ShowIf(nameof(_enableArmor))] private float _maxArmor = 50f;

		public event Action<float> ArmorDamaged;
		public event Action<float> HealthDamaged;
		public event Action<float> Healed;
		public event Action Died;

		private bool _isDead = false;

		public float CurrentHealth { get; private set; }

		public float CurrentArmor { get; private set; }

		public float MaxHealth => _maxHealth;

		public bool IsDead => _isDead;

		private void Start()
		{
			CurrentHealth = _maxHealth;
		}

		public void Ressurect()
		{
			_isDead = false;

			CurrentArmor = _maxArmor;

			CurrentHealth = _maxHealth;
		}

		public void SetMaxHealth(float maxHealth)
		{
			if (maxHealth <= 0f)
				throw new ArgumentOutOfRangeException(nameof(maxHealth));

			_maxHealth = maxHealth;

			CurrentHealth = _maxHealth;
		}

		public void Heal(float healAmount)
		{
			if (healAmount < 0)
				throw new ArgumentOutOfRangeException(nameof(healAmount));

			float healthBefore = CurrentHealth;
			CurrentHealth += healAmount;
			CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, _maxHealth);

			float trueHealAmount = CurrentHealth - healthBefore;
			if (trueHealAmount > 0f)
				Healed?.Invoke(trueHealAmount);
		}

		public void TakeDamage(float damage)
		{
			if (damage < 0)
				throw new ArgumentOutOfRangeException(nameof(damage));

			if (Invincible)
				return;

			if (_enableArmor && CurrentArmor > 0f)
			{
				ArmorDamage(damage);

				return;
			}

			HeathDamage(damage);
		}

		public void Kill()
		{
			CurrentArmor = CurrentHealth = 0f;

			ArmorDamaged?.Invoke(CurrentArmor);

			HealthDamaged?.Invoke(CurrentHealth);

			HandleDeath();
		}

		private void HeathDamage(float damage)
		{
			float healthBefore = CurrentHealth;
			CurrentHealth -= damage;
			CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, _maxHealth);

			float trueDamageAmount = healthBefore - CurrentHealth;

			if (trueDamageAmount > 0f)
				HealthDamaged?.Invoke(trueDamageAmount);

			HandleDeath();
		}

		private void ArmorDamage(float damage)
		{
			float armorBefore = CurrentArmor;
			CurrentArmor -= damage;
			CurrentArmor = Mathf.Clamp(CurrentArmor, 0f, _maxArmor);

			float trueDamageAmount = armorBefore - CurrentArmor;

			if (trueDamageAmount > 0f)
				ArmorDamaged?.Invoke(trueDamageAmount);
		}

		private void HandleDeath()
		{
			if (_isDead)
				return;

			if (CurrentHealth <= 0f)
			{
				_isDead = true;
				Died?.Invoke();
			}
		}
	}
}