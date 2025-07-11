using EFK2.Game.Save;
using EFK2.Inputs.Interfaces;
using System;
using UnityEngine;

namespace EFK2.Inputs
{
	public sealed class MouseInputService : IMouseInputService, IMouseSensivityService
	{
		private float? _mouseSensivity;

		private readonly bool _hideCursor = true;
		private readonly bool _lockCursor = true;

		private const string _mouseSensivityKey = "Sensivity";

		private const float DefaultSensivity = 3f;

		float IMouseSensivityService.MouseSensivity => GetMouseSensivityInternal();

		IMouseSensivityService IMouseInputService.MouseSensivityService => this;

		float IMouseInputService.GetMouseLookX()
		{
			return Input.GetAxisRaw(InputConstants.mouseLookX);
		}

		float IMouseInputService.GetMouseLookY()
		{
			return Input.GetAxisRaw(InputConstants.mouseLookY);
		}

		void IMouseInputService.SetCursorState(bool state)
		{
			Cursor.lockState = state && _lockCursor ? CursorLockMode.None : CursorLockMode.Locked;

			Cursor.visible = state && _hideCursor;
		}

		void IMouseSensivityService.SetMouseSensivity(float mouseSensivity)
		{
			AssignNewSensivity(mouseSensivity);
		}

		void IMouseSensivityService.ResetSensivity(float defaultSensivity)
		{
			SaveUtility.DeleteKey(_mouseSensivityKey);

			AssignNewSensivity(defaultSensivity);
		}

		private void AssignNewSensivity(float mouseSensivity)
		{
			if (mouseSensivity <= 0)
				throw new ArgumentOutOfRangeException(nameof(mouseSensivity));

			_mouseSensivity = mouseSensivity;

			SaveUtility.SaveData(_mouseSensivityKey, mouseSensivity);
		}

		private float GetMouseSensivityInternal()
		{
			_mouseSensivity ??= SaveUtility.LoadData(_mouseSensivityKey, DefaultSensivity);

			return _mouseSensivity.Value;
		}
	}
}