using Cysharp.Threading.Tasks;
using EFK2.Game.ResetSystem;
using EFK2.Inputs.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace EFK2.Settings.InputSettings
{
	public class InputUpdatePresenter : MonoBehaviour, IResetable
	{
		[Header("Source")]
		[SerializeField] private Button _controlKeyButton;
		[SerializeField] private TMP_Text _buttonText;

		[Header("Key")]
		[SerializeField] private KeyCode _defaultControlKey;
		[SerializeField] private string _savedControlKey;

		private UniTask _currentTask;

		private ResetService _resetService;

		private IMouseInputService _mouseInput;
		private IKeyboardUpdateService _keyboardUpdateService;
		private IKeyboardInputService _keyboardInputService;

		private const string _waitingForInputConst = "...";

		private void Awake()
		{
			_buttonText.text = _keyboardUpdateService.KeyValues[_savedControlKey].ToString();
		}

		private void OnEnable()
		{
			_resetService.Register(this);

			_controlKeyButton.onClick.AddListener(ChangeControlKey);
		}

		private void OnDisable()
		{
			_resetService.Unregister(this);

			_controlKeyButton.onClick.RemoveListener(ChangeControlKey);
		}

		[Inject]
		public void Construct(IMouseInputService mouseInput, IKeyboardInputService keyboardInputService, ResetService resetService)
		{
			_resetService = resetService;

			_mouseInput = mouseInput;

			_keyboardUpdateService = keyboardInputService.UpdateService;

			_keyboardInputService = keyboardInputService;
		}

		private void ChangeControlKey()
		{
			if (_currentTask.Status.IsCompleted() == false)
				return;

			_currentTask = WaitForKeyInput();
		}

		private async UniTask WaitForKeyInput()
		{
			string newText = _buttonText.text;

			_buttonText.text = _waitingForInputConst;

			_mouseInput.SetCursorState(false);

			await UniTask.WaitUntil(() =>
			{
				if (_keyboardInputService.GetCurrentPressedKey(out KeyCode tempKey))
				{
					if (_keyboardUpdateService.SetKey(_savedControlKey, tempKey))
						newText = tempKey.ToString();

					_buttonText.text = newText;

					_mouseInput.SetCursorState(true);

					return true;
				}

				return false;
			});
		}

		void IResetable.Reset()
		{
			_keyboardUpdateService.ResetKey(_savedControlKey, _defaultControlKey);

			_buttonText.text = _defaultControlKey.ToString();
		}
	}
}