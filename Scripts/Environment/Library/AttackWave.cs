using EFK2.AI.StateMachines;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EFK2.Environment
{
	[CreateAssetMenu(fileName = "New Attack Wave", menuName = "Source/New Attack Wave", order = 51)]
	public class AttackWave : ScriptableObject
	{
		[Header("Enemies")]
		[SerializeField] private List<EnemyType> _enemyOnWaves;

		public List<EnemyType> EnemyOnWaves => _enemyOnWaves;
	}

	[Serializable]
	public struct EnemyType
	{
		public EnemyStateMachine enemy;
		public int enemiesCountShouldBeSpawned;
	}
}