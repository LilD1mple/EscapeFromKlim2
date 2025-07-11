using EFK2.Difficult.Configurations;
using UnityEngine;

namespace EFK2.Difficult
{
	[CreateAssetMenu(fileName = "New Difficult Level", menuName = "Source/Create New Difficult Level", order = 0)]
	public class DifficultLevelConfiguration : ScriptableObject
	{
		[Header("Main")]
		[SerializeField] private string _difficultName = "New Difficult";

		[Header("Player")]
		[SerializeField, Min(1)] private float _playerHealth;

		[Header("Spawn Settings")]
		[SerializeField, Min(0)] private float _enemySpawnDelay;
		[SerializeField, Min(0)] private float _healItemSpawnInterval;

		[Header("Characters Settings")]
		[SerializeField] private EnemyConfig _warriorConfig;
		[SerializeField] private EnemyConfig _fireWizardConfig;
		[SerializeField] private EnemyConfig _waterWizardConfig;
		[SerializeField] private EnemyConfig _flashKamikazeConfig;
		[SerializeField] private EnemyConfig _meleeBerserkConfig;
		[SerializeField] private EnemyConfig _berserkConfig;
		[SerializeField] private EnemyConfig _dualBladeConfig;
		[SerializeField] private BossEnemyConfig _supermagicianWizardConfig;

		[Header("Spells Settings")]
		[SerializeField] private SpellConfig _lightningSpellConfig;
		[SerializeField] private SpellConfig _temporacySpellConfig;

		[Header("Enables")]
		[SerializeField] private bool _enableSpawnHealItems = true;
		[SerializeField] private bool _enableSpecialSkills = true;

		public string DifficultName => _difficultName;

		public float EnemySpawnDelay => _enemySpawnDelay;

		public float HealItemSpawnInterval => _healItemSpawnInterval;

		public float PlayerHealth => _playerHealth;

		public bool EnableSpawnHealItems => _enableSpawnHealItems;

		public bool EnableSpecialSkills => _enableSpecialSkills;

		public EnemyConfig WarriorConfig => _warriorConfig;

		public EnemyConfig WaterWizardConfig => _waterWizardConfig;

		public EnemyConfig FireWizardConfig => _fireWizardConfig;

		public EnemyConfig FlashKamikazeConfig => _flashKamikazeConfig;

		public EnemyConfig MeleeBerserkConfig => _meleeBerserkConfig;

		public EnemyConfig BerserkConfig => _berserkConfig;

		public EnemyConfig DualBladeConfig => _dualBladeConfig;

		public BossEnemyConfig SupermagicianWizardConfig => _supermagicianWizardConfig;

		public SpellConfig LightningSpellConfig => _lightningSpellConfig;

		public SpellConfig TemporacySpellConfig => _temporacySpellConfig;
	}
}