using EFK2.Events;
using EFK2.Events.Signals;
using EFK2.Extensions;
using EFK2.Game.Save;
using EFK2.Inputs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace EFK2.Inputs
{
	public sealed class KeyboardInputService : IKeyboardInputService, IKeyboardUpdateService
	{
		private const string ArgumentException = "Похоже, что вы пытаетесь добавить ключ, которого не должно быть!";

		private readonly Dictionary<string, KeyCode> _keyValuePairs = new()
		{
			{ InputConstants.runKeyConst, KeyCode.LeftShift },
			{ InputConstants.crouchKeyConst, KeyCode.LeftControl },
			{ InputConstants.interactKeyConst, KeyCode.E },
			{ InputConstants.flashlightKeyConst, KeyCode.F },
			{ InputConstants.throwKeyConst, KeyCode.Tab },
			{ InputConstants.jumpKeyConst, KeyCode.Space },
			{ InputConstants.attackKeyConst, KeyCode.Mouse0 },
			{ InputConstants.lightningSpellAttackKeyConst, KeyCode.R },
			{ InputConstants.temporacyExplosionSpellAttackKeyConst, KeyCode.T },
			{ InputConstants.pauseKey, KeyCode.Escape }
		};

		private readonly EventBus _eventBus;

		[Inject]
		public KeyboardInputService(EventBus eventBus)
		{
			_eventBus = eventBus;

			LoadKeys();
		}

		public IKeyboardUpdateService UpdateService => this;

		public IReadOnlyDictionary<string, KeyCode> KeyValues => _keyValuePairs;

		public Vector2 GetMoveInput()
		{
			float moveX = Input.GetAxisRaw(InputConstants.horizontalAxis);
			float moveZ = Input.GetAxisRaw(InputConstants.verticalAxis);

			return new Vector2(moveZ, moveX);
		}

		public bool GetCurrentPressedKey(out KeyCode pressedKey)
		{
			Array allKeys = Enum.GetValues(typeof(KeyCode));

			foreach (KeyCode tempKey in allKeys)
			{
				if (Input.GetKeyDown(tempKey))
				{
					pressedKey = tempKey;

					return true;
				}
			}

			pressedKey = default;

			return false;
		}

		public bool GetPressedKey(string key) => Input.GetKey(_keyValuePairs[key]);

		public bool GetPressedKeyDown(string key) => Input.GetKeyDown(_keyValuePairs[key]);

		public bool GetPressedKeyDown(KeyCode keyCode) => Input.GetKeyDown(keyCode);

		public bool SetKey(string key, KeyCode keyCode)
		{
			if (_keyValuePairs.ContainsKey(key) == false)
				throw new ArgumentException(ArgumentException);

			if (_keyValuePairs.ContainsValue(keyCode))
				return false;

			AssignNewKey(key, keyCode);

			return true;
		}

		public void ResetKey(string key, KeyCode defaultKey)
		{
			SaveUtility.DeleteKey(key);

			AssignNewKey(key, defaultKey);
		}

		private void AssignNewKey(string key, KeyCode keyCode)
		{
			_keyValuePairs[key] = keyCode;

			SaveUtility.SaveData(key, keyCode.ToString());

			_eventBus.Raise(new InputKeyChangedSignal(key, keyCode));
		}

		private void LoadKeys()
		{
			string[] keys = _keyValuePairs.Keys.ToArray();

			for (int i = 0; i < keys.Length; i++)
			{
				string key = keys[i];

				KeyCode oldKey = _keyValuePairs[key];

				KeyCode newKey = key.ParseSavedKey(oldKey);

				_keyValuePairs[key] = newKey;
			}
		}
	}
}