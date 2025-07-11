using EFK2.Game.ResetSystem;
using EFK2.Game.Save;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace EFK2.Settings.Display
{
    public class FPSCounterSettingPresenter : MonoBehaviour, IResetable
    {
        [Header("Main")]
        [SerializeField] private Toggle _fpsCounterToggle;

        [Header("Source")]
        [SerializeField] private TMP_Text _fpsCounterText;

        [Inject] private readonly ResetService _resetService;

        private const string _fpsCounter = "FPSCounter";

        private void Awake()
        {
            bool enabled = SaveUtility.LoadData(_fpsCounter, true);

            _fpsCounterToggle.isOn = enabled;

            OnToggleValueChanged(enabled);
        }

        private void OnEnable()
        {
            _resetService.Register(this);

            _fpsCounterToggle.onValueChanged.AddListener(OnToggleValueChanged);
        }

        private void OnDisable()
        {
            _resetService.Unregister(this);

            _fpsCounterToggle.onValueChanged.RemoveListener(OnToggleValueChanged);
        }

        private void OnToggleValueChanged(bool enable)
        {
            _fpsCounterText.enabled = enable;

            SaveUtility.SaveData(_fpsCounter, enable);
        }

        void IResetable.Reset()
        {
            SaveUtility.DeleteKey(_fpsCounter);

            _fpsCounterToggle.isOn = true;

            OnToggleValueChanged(true);
        }
    }
}