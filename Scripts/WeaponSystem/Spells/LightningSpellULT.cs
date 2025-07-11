using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace EFK2.WeaponSystem.Spells
{
	public class LightningSpellULT : Spell
	{
		[Header("Source")]
		[SerializeField] private LightningSpell[] _lightningSpells;

		[Header("Settings")]
		[SerializeField] private float _spawnInterval;

		public override void StartAttack()
		{
			for (int i = 0; i < _lightningSpells.Length; i++)
			{
				_lightningSpells[i].SetDamage(Damage);
			}

			SpawnLightnings().Forget();
		}

		private async UniTaskVoid SpawnLightnings()
		{
			for (int i = 0; i < _lightningSpells.Length; i++)
			{
				_lightningSpells[i].StartAttack();

				await UniTask.Delay(TimeSpan.FromSeconds(_spawnInterval));
			}
		}
	}
}