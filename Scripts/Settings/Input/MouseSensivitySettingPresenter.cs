using EFK2.Game.ResetSystem;
using EFK2.Inputs.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace EFK2.Settings.InputSettings
{
    public class MouseSensivitySettingPresenter : MonoBehaviour, IResetable
    {
        [Header("Source")]
        [SerializeField] private Slider _sensivitySlider;

        [Header("View")]
        [SerializeField] private MouseSensivitySettingsViewer _sensivitySettingsViewer;

        private ResetService _resetService;

        private IMouseSensivityService _mouseSensivityService;

        private void Start()
        {
            float value = _mouseSensivityService.MouseSensivity;

            _sensivitySlider.value = value;

            ChangeSensivity(value);
        }

        private void OnEnable()
        {
            _resetService.Register(this);

            _sensivitySlider.onValueChanged.AddListener(ChangeSensivity);
        }

        private void OnDisable()
        {
            _resetService.Unregister(this);

            _sensivitySlider.onValueChanged.RemoveListener(ChangeSensivity);
        }

        [Inject]
        public void Construct(ResetService resetService, IMouseInputService mouseInputService)
        {
            _resetService = resetService;

            _mouseSensivityService = mouseInputService.MouseSensivityService;
        }

        private void ChangeSensivity(float sensivity)
        {
            _mouseSensivityService.SetMouseSensivity(sensivity);

            _sensivitySettingsViewer.RefreshUI(sensivity);
        }

        void IResetable.Reset()
        {
            _mouseSensivityService.ResetSensivity(3f);

            _sensivitySlider.value = 3f;
        }
    }
}
