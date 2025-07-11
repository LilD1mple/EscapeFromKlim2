using EFK2.Debugs;
using EFK2.Game.ResetSystem;
using EFK2.Game.Save;
using Michsky.UI.Dark;
using UnityEngine;
using Zenject;

namespace EFK2.Settings.Debug
{
	public class DebugLoggerSetting : MonoBehaviour, IResetable
	{
		[SerializeField] private SwitchManager _switchManager;

		private DebugLogger _debugLogger;
		private ResetService _resetService;

		private const string _debugLoggerKey = "Debug";

		private void OnEnable()
		{
			_resetService.Register(this);

			_switchManager.EnableChange += SetDebugLoggerSave;
		}

		private void OnDisable()
		{
			_resetService.Unregister(this);

			_switchManager.EnableChange -= SetDebugLoggerSave;
		}

		private void Start()
		{
			bool enable = SaveUtility.LoadData(_debugLoggerKey, false);

			_debugLogger.CanSaveResult = enable;

			_switchManager.isOn = !enable;

			_switchManager.AnimateSwitch();
		}

		[Inject]
		public void Construct(DebugLogger debugLogger, ResetService resetService)
		{
			_debugLogger = debugLogger;

			_resetService = resetService;
		}

		private void SetDebugLoggerSave(bool enable)
		{
			_debugLogger.CanSaveResult = enable;

			SaveUtility.SaveData(_debugLoggerKey, enable);
		}

		void IResetable.Reset()
		{
			SaveUtility.DeleteKey(_debugLoggerKey);

			_switchManager.isOn = true;

			_debugLogger.CanSaveResult = false;

			_switchManager.AnimateSwitch();
		}
	}
}