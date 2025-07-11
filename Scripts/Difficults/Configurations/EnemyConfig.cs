using System;
using UnityEngine;

namespace EFK2.Difficult.Configurations
{
	[Serializable]
	public class EnemyConfig
	{
		[Header("Body")]
		[SerializeField] private float _movementSpeed;
		[SerializeField] private float _maxEnemyHealth;

		[Header("Attack")]
		[SerializeField] private float _damage;
		[SerializeField] private float _maxAttackDistance;

		public float Damage => _damage;

		public float MovementSpeed => _movementSpeed;

		public float MaxAttackDistance => _maxAttackDistance;

		public float MaxEnemyHealth => _maxEnemyHealth;
	}
}
