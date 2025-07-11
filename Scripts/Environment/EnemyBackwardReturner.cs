using EFK2.AI.StateMachines;
using UnityEngine;

namespace EFK2.Environment
{
	public class EnemyBackwardReturner : MonoBehaviour
	{
		[Header("Points")]
		[SerializeField] private Transform[] _backPoints;

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out EnemyStateMachine enemy))
			{
				enemy.transform.position = _backPoints[Random.Range(0, _backPoints.Length)].position;
			}
		}
	}
}