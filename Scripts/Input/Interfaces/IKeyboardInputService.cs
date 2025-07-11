using UnityEngine;

namespace EFK2.Inputs.Interfaces
{
	public interface IKeyboardInputService
	{
		IKeyboardUpdateService UpdateService { get; }

		bool GetPressedKey(string key);

		bool GetPressedKeyDown(string key);

		bool GetPressedKeyDown(KeyCode keyCode);

		bool GetCurrentPressedKey(out KeyCode pressedKey);

		Vector2 GetMoveInput();
	}
}