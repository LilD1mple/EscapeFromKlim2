using System;
using UnityEngine;

namespace EFK2.Difficult.Configurations
{
	[Serializable]
	public sealed class BossEnemyConfig : EnemyConfig
	{
		[SerializeField] private float _specialAttackDamage;

		public float SpecialAttackDamage => _specialAttackDamage;
	}
}
