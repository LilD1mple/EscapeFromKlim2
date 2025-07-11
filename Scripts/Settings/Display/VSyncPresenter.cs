using EFK2.Game.ResetSystem;
using EFK2.Game.Save;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace EFK2.Settings.Display
{
    public class VSyncPresenter : MonoBehaviour, IResetable
    {
        [SerializeField] private Toggle _vSyncToggle;

        [Inject] private readonly ResetService _resetService;

        private const string _vSyncEnableConst = "VSync";

        private void Start()
        {
            bool enabled = SaveUtility.LoadData(_vSyncEnableConst, true);

            _vSyncToggle.isOn = enabled;

            ChangeVSyncEnable(enabled);
        }

        private void OnEnable()
        {
            _resetService.Register(this);

            _vSyncToggle.onValueChanged.AddListener(ChangeVSyncEnable);
        }

        private void OnDisable()
        {
            _resetService.Unregister(this);

            _vSyncToggle.onValueChanged.RemoveListener(ChangeVSyncEnable);
        }

        private void ChangeVSyncEnable(bool enable)
        {
            QualitySettings.vSyncCount = enable ? 1 : 0;

            SaveUtility.SaveData(_vSyncEnableConst, enable);
        }

        void IResetable.Reset()
        {
            SaveUtility.DeleteKey(_vSyncEnableConst);

            _vSyncToggle.isOn = true;

            ChangeVSyncEnable(true);
        }
    }
}
