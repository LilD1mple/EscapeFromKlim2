using EFK2.Difficult.Configurations;
using EFK2.Game.PauseSystem;
using EFK2.Game.StartSystem;
using EFK2.Inputs.Interfaces;
using EFK2.Player.Aim;
using NTC.Pool;
using System;
using UnityEngine;
using Zenject;

namespace EFK2.WeaponSystem.Spells
{
	public class SpellPresenter : MonoBehaviour, IPauseable, IStartable
	{
		[Header("Magic Spell Prefab")]
		[SerializeField] private Spell _magicSpellPrefab;

		[Header("Spawn Pivot")]
		[SerializeField] private Transform _attackPoint;

		[Header("Settings")]
		[SerializeField] private string _activateKey;
		[SerializeField, Min(1f)] private float _prefabLifeTime;

		private PauseService _pauseService;
		private StartableService _startableService;

		private IKeyboardInputService _keyboardInputService;
		private IAimService _aimService;

		private float _regenerationTime;
		private float _timeComplete = 0f;

		private bool _canAttack = false;
		private bool _isPreCastEnabled = false;
		private bool _isPaused = false;
		private bool _isGameStarted = false;

		public event Action<float> RemainingTimeChanged;
		public event Action<bool> ChargeChanged;

		private void Start()
		{
			ChargeChanged?.Invoke(_canAttack);
		}

		private void OnEnable()
		{
			_startableService.Register(this);

			_pauseService.Register(this);
		}

		private void OnDisable()
		{
			_startableService.Unregister(this);

			_pauseService.Unregister(this);
		}

		private void Update()
		{
			if (_isPaused || _isGameStarted == false)
				return;

			CheckInput();

			CalculateRemainigTime();
		}

		[Inject]
		public void Construct(PauseService pauseService, StartableService startableService, IAimService aimService, IKeyboardInputService keyboardInputService)
		{
			_pauseService = pauseService;

			_startableService = startableService;

			_keyboardInputService = keyboardInputService;

			_aimService = aimService;
		}

		public void LoadConfig(SpellConfig spellConfig)
		{
			_regenerationTime = spellConfig.RegenerationTime;

			_magicSpellPrefab.SetDamage(spellConfig.Damage);
		}

		private void CalculateRemainigTime()
		{
			if (_canAttack == false)
			{
				_timeComplete += Time.deltaTime;

				if (_timeComplete >= _regenerationTime)
				{
					_canAttack = true;

					ChargeChanged?.Invoke(_canAttack);
				}

				RemainingTimeChanged?.Invoke(Mathf.InverseLerp(0f, _regenerationTime, _timeComplete));
			}
		}

		private void CheckInput()
		{
			if (_keyboardInputService.GetPressedKeyDown(_activateKey) && _isPreCastEnabled)
				SpawnSpell();

			if (_canAttack && _keyboardInputService.GetPressedKeyDown(_activateKey))
				ChangePreCastState(true);
		}

		private void ChangePreCastState(bool enable)
		{
			_isPreCastEnabled = enable;

			_aimService.SetTargetAim(enable ? AimType.TargetMarker : AimType.Crosshair);
		}

		private void SpawnSpell()
		{
			Spell magicSpell = NightPool.Spawn(_magicSpellPrefab, _attackPoint.position, Quaternion.identity);

			_canAttack = false;

			_timeComplete = 0f;

			ChargeChanged?.Invoke(_canAttack);

			ChangePreCastState(false);
		}

		void IPauseable.SetPause(bool isPaused)
		{
			_isPaused = isPaused;
		}

		void IStartable.StartGame()
		{
			_isGameStarted = true;
		}
	}
}