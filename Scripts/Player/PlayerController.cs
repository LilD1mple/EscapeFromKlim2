using Cysharp.Threading.Tasks;
using EFK2.Events;
using EFK2.Events.Interfaces;
using EFK2.Events.Signals;
using EFK2.Game.PauseSystem;
using EFK2.Inputs;
using EFK2.Inputs.Interfaces;
using EFK2.Interact;
using NaughtyAttributes;
using System;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace EFK2.Player
{
	[SelectionBase]
	[RequireComponent(typeof(AudioSource))]
	public class PlayerController : Unit, IPauseable, IEventReceiver<PlayerDiedSignal>
	{
		#region Fields

		[Header("Components")]
		[SerializeField] private Camera _mainCamera;
		[SerializeField] private CharacterController _characterController;
		[SerializeField] private Transform _cameraHolder;

		[Header("Movement Settings")]
		[SerializeField, Range(1f, 20f)] private float _playerWalkSpeed = 3f;
		[SerializeField] private float _minRotateY = -90f;
		[SerializeField] private float _maxRotateY = 90f;

		[Header("Footstep Sounds")]
		[SerializeField] private bool _footStepSoundsEnabled = true;
		[SerializeField, ShowIf(nameof(_footStepSoundsEnabled))] private AudioClip[] _footStepClips;
		[SerializeField, Range(0f, 5f), ShowIf(nameof(_footStepSoundsEnabled))] private float _footStepSoundsDelay = 0.5f;
		[SerializeField, Range(0f, 10f), ShowIf(nameof(_footStepSoundsEnabled))] private float _minMoveFootStepMagnitude = 0.1f;
		[SerializeField, ShowIf(nameof(_footStepSoundsEnabled))] private float _delayFootStepSoundsOnRun = 0.4f;
		[SerializeField, ShowIf(nameof(_footStepSoundsEnabled))] private bool _randomPitchSoundEnabled = true;
		[SerializeField, ShowIf(nameof(_footStepSoundsEnabled))] private Vector2 _randomPitchRange = new(0.8f, 1f);

		[Header("Run Settings")]
		[SerializeField] private bool _runEnabled = true;
		[SerializeField, Range(1f, 30f), ShowIf(nameof(_runEnabled))] private float _runSpeed = 5f;

		[Header("Stamina Settings")]
		[SerializeField] private bool _staminaEnabled = true;
		[SerializeField, Range(1f, 100f), ShowIf(nameof(_staminaEnabled))] private float _staminaCount = 50f;
		[SerializeField, ShowIf(nameof(_staminaEnabled))] private float _staminaCost = 1f;
		[SerializeField, ShowIf(nameof(_staminaEnabled))] private float _staminaDelayRegeneration = 5f;
		[SerializeField, ShowIf(nameof(_staminaEnabled))] private float _regenerationConst = 1f;

		[Header("Jump Settings")]
		[SerializeField] private bool _jumpEnabled = true;
		[SerializeField, ShowIf(nameof(_jumpEnabled))] private float _jumpSpeed = 2f;
		[SerializeField, ShowIf(nameof(_jumpEnabled))] private float _gravityDownForce = 10f;

		[Header("Headbob Settings")]
		[SerializeField] private bool _headBobEnabled = true;
		[SerializeField, Range(0f, 0.1f), ShowIf(nameof(_headBobEnabled))] private float _amplitude = 0.015f;
		[SerializeField, Range(0f, 30f), ShowIf(nameof(_headBobEnabled))] private float _frequency = 10f;
		[SerializeField, ShowIf(nameof(_headBobEnabled))] private float _toggleSpeed = 3f;
		[SerializeField, ShowIf(nameof(_headBobEnabled))] private float _backSpeed = 1f;

		[Header("FOVKick Settings")]
		[SerializeField] private bool _fovKickEnabled = true;
		[SerializeField, Range(1f, 179f), ShowIf(nameof(_fovKickEnabled))] private float _fieldOfView = 65f;
		[SerializeField, Range(1f, 30f), ShowIf(nameof(_fovKickEnabled))] private float _fovKickSpeed = 10f;
		[SerializeField, Range(0f, 30f), ShowIf(nameof(_fovKickEnabled))] private float _fovKickfrequency = 12f;

		[Header("Crouch Settings")]
		[SerializeField] private bool _crouchEnabled = true;
		[SerializeField, Range(0f, 10f), ShowIf(nameof(_crouchEnabled))] private float _crouchSpeed = 1.8f;
		[SerializeField, ShowIf(nameof(_crouchEnabled))] private float _crouchHeight = 1f;
		[SerializeField, ShowIf(nameof(_crouchEnabled))] private float _crouchTransitionSpeed = 2f;

		[Header("Interactable Settings")]
		[SerializeField] private bool _interactEnabled = true;

		private PauseService _pauseService;
		private EventBus _eventBus;

		private IKeyboardInputService _keyboardInput;
		private IMouseInputService _mouseInput;
		private IRaycastService _raycastService;

		private Vector3 _startPosition;
		private Vector3 _startCameraPosition;
		private Vector3 _characterVelocity;

		private float _characterVelocityY;
		private float _cameraVerticalAngle;
		private float _startFieldOfView;
		private float _startFrequency;
		private float _startHeight;
		private float _currentStaminaCount;
		private float _currentHeight;

		private int _currentClipStep;

		private bool _getPresedRunKey = false;
		private bool _canPlayFootStepSounds = true;
		private bool _playerDied = false;

		private AudioSource _footStepSoundSource;

		private UniTask _currentRegenerationTask;

		#endregion

		#region Properties

		private bool CanPlayerRun => _currentStaminaCount > 0f && IsCrouching == false;

		private bool IsCrouching => _startHeight - _currentHeight > .3f;

		public bool IsPaused { get; private set; } = false;

		public Camera MainCamera => _mainCamera;

		UniqueId IBaseEventReceiver.Id => new();

		#endregion

		#region Events

		public event Action<float, float> StaminaChange;

		#endregion

		#region Constructor

		[Inject]
		public void Construct(PauseService pauseService, EventBus eventBus, IKeyboardInputService keyboardInputService, IMouseInputService mouseInput)
		{
			_pauseService = pauseService;

			_eventBus = eventBus;

			_keyboardInput = keyboardInputService;

			_mouseInput = mouseInput;
		}

		#endregion

		#region UnityMethods

		private void OnEnable()
		{
			_pauseService.Register(this);

			_eventBus.Subscribe(this);
		}

		private void OnDisable()
		{
			_eventBus.Unsubscribe(this);

			_pauseService.Unregister(this);
		}

		private void Start()
		{
			GetComponents();

			_currentStaminaCount = _staminaCount;

			_startHeight = _characterController.height;

			_startFrequency = _frequency;

			_startFieldOfView = _mainCamera.fieldOfView;

			_startPosition = _mainCamera.transform.localPosition;

			_startCameraPosition = _cameraHolder.transform.localPosition;

			_mouseInput.SetCursorState(false);
		}

		private void Update()
		{
			if (IsPaused || _playerDied)
				return;

			HandleCharacterLook();

			HandleCharacterMovement();

			Crouching();

			UpdateStamina();

			CheckStaminaCount();

			DynamicFOVKick();

			Interact();

			if (_headBobEnabled == false)
				return;

			CheckMotion();

			ResetPosition();

			//_mainCamera.transform.LookAt(FocusTarget());
		}

		#endregion

		#region Methods

		protected override void HandleCharacterLook()
		{
			float lookX = _mouseInput.GetMouseLookX();
			float lookY = _mouseInput.GetMouseLookY();

			transform.Rotate(new Vector3(0f, lookX * _mouseInput.MouseSensivityService.MouseSensivity, 0f), Space.Self);

			_cameraVerticalAngle -= lookY * _mouseInput.MouseSensivityService.MouseSensivity;

			_cameraVerticalAngle = Mathf.Clamp(_cameraVerticalAngle, _minRotateY, _maxRotateY);

			_mainCamera.transform.localEulerAngles = new Vector3(_cameraVerticalAngle, 0f, 0f);
		}

		protected override void HandleCharacterMovement()
		{
			_getPresedRunKey = _keyboardInput.GetPressedKey(InputConstants.runKeyConst);

			float playerSpeed = _getPresedRunKey && CanPlayerRun && _runEnabled ? _runSpeed : _playerWalkSpeed;

			if (IsCrouching)
				playerSpeed = _crouchSpeed;

			Vector2 currentInput = _keyboardInput.GetMoveInput() * playerSpeed;

			_characterVelocityY = _characterVelocity.y;
			_characterVelocity = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y);
			_characterVelocity.y = _characterVelocityY;

			if (_characterController.isGrounded && _jumpEnabled)
				_characterVelocity.y = _keyboardInput.GetPressedKey(InputConstants.jumpKeyConst) ? _jumpSpeed : 0f;

			if (_characterController.isGrounded == false)
				_characterVelocity.y -= _gravityDownForce * Time.deltaTime;

			if (_footStepSoundsEnabled && _characterVelocity.magnitude >= _minMoveFootStepMagnitude && _canPlayFootStepSounds && _characterController.isGrounded)
				PlayFootStepSounds();

			_characterController.Move(_characterVelocity * Time.deltaTime);
		}

		private void GetComponents()
		{
			if (_footStepSoundsEnabled)
				_footStepSoundSource = GetComponent<AudioSource>();

			if (_interactEnabled)
				_raycastService = GetComponent<RaycastService>();
		}

		private void Crouching()
		{
			if (_crouchEnabled == false)
				return;

			float heightTarget = _keyboardInput.GetPressedKey(InputConstants.crouchKeyConst) ? _crouchHeight : _startHeight;

			_currentHeight = Mathf.Lerp(_currentHeight, heightTarget, _crouchTransitionSpeed * Time.deltaTime);

			Vector3 halfHeightDifference = new Vector3(0f, (_startHeight - _currentHeight) / 2, 0f);
			Vector3 newCameraPosition = _startCameraPosition - halfHeightDifference;

			_cameraHolder.transform.localPosition = newCameraPosition;
			_characterController.height = _currentHeight;
		}

		private void PlayMotion(Vector3 motion)
		{
			_mainCamera.transform.localPosition += motion;
		}

		private void CheckMotion()
		{
			float speed = new Vector3(_characterController.velocity.x, 0f, _characterController.velocity.z).magnitude;

			if (speed < _toggleSpeed || _characterController.isGrounded == false)
				return;

			PlayMotion(FootStepMotion());
		}

		private void ResetPosition()
		{
			if (_mainCamera.transform.localPosition == _startPosition)
				return;

			_mainCamera.transform.localPosition = Vector3.Lerp(_mainCamera.transform.localPosition, _startPosition, _backSpeed * Time.deltaTime);
		}

		private Vector3 FootStepMotion()
		{
			Vector3 position = Vector3.zero;

			position.y += Mathf.Sin(Time.time * _frequency) * _amplitude;

			position.x += Mathf.Cos(Time.time * _frequency / 2) * _amplitude * 2;

			return position;
		}

		private Vector3 FocusTarget()
		{
			Vector3 position = new Vector3(transform.position.x, transform.position.y + _cameraHolder.localPosition.y, transform.position.z);
			position += transform.forward * 15f;
			return position;
		}

		private void DynamicFOVKick()
		{
			if (_fovKickEnabled == false)
				return;

			ChangeStateFOVKick();
		}

		private void ChangeStateFOVKick()
		{
			float fieldOfView = _getPresedRunKey && CanPlayerRun && _runEnabled ? _fieldOfView : _startFieldOfView;

			_frequency = _getPresedRunKey && CanPlayerRun && _runEnabled ? _fovKickfrequency : _startFrequency;

			_mainCamera.fieldOfView = Mathf.Lerp(_mainCamera.fieldOfView, fieldOfView, _fovKickSpeed * Time.deltaTime);
		}

		private void PlayFootStepSounds()
		{
			if (IsCrouching)
				return;

			_footStepSoundSource.clip = _footStepClips[_currentClipStep];

			if (_randomPitchSoundEnabled)
				_footStepSoundSource.pitch = Random.Range(_randomPitchRange.x, _randomPitchRange.y);

			_footStepSoundSource.Play();

			FootStepDelay().Forget();
		}

		private void Interact()
		{
			if (_interactEnabled && _keyboardInput.GetPressedKey(InputConstants.interactKeyConst))
				_raycastService.Raycast();
		}

		private void UpdateStamina()
		{
			if (_staminaEnabled == false || _getPresedRunKey == false || CanPlayerRun == false)
				return;

			_currentStaminaCount -= _staminaCost * Time.deltaTime;

			StaminaChange?.Invoke(_staminaCount, _currentStaminaCount);
		}

		private void CheckStaminaCount()
		{
			if (_currentStaminaCount < _staminaCount && _currentRegenerationTask.Status.IsCompleted() && _getPresedRunKey == false)
				_currentRegenerationTask = RegenerateStamina();
		}

		private async UniTaskVoid FootStepDelay()
		{
			_canPlayFootStepSounds = false;

			await UniTask.Delay(TimeSpan.FromSeconds(_getPresedRunKey && CanPlayerRun ? _delayFootStepSoundsOnRun : _footStepSoundsDelay));

			_canPlayFootStepSounds = true;

			_currentClipStep++;

			if (_currentClipStep > _footStepClips.Length - 1)
				_currentClipStep = 0;
		}

		private async UniTask RegenerateStamina()
		{
			_currentStaminaCount += _regenerationConst;

			if (_currentStaminaCount > _staminaCount)
				_currentStaminaCount = _staminaCount;

			StaminaChange?.Invoke(_staminaCount, _currentStaminaCount);

			await UniTask.Delay(TimeSpan.FromSeconds(_staminaDelayRegeneration));
		}

		#endregion

		void IPauseable.SetPause(bool isPaused)
		{
			IsPaused = isPaused;

			_mouseInput.SetCursorState(isPaused);
		}

		void IEventReceiver<PlayerDiedSignal>.OnEvent(PlayerDiedSignal @event)
		{
			_playerDied = true;
		}
	}
}
