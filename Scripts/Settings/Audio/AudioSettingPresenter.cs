using EFK2.Audio.Interfaces;
using EFK2.Game.ResetSystem;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace EFK2.Settings.Audio
{
    public class AudioSettingPresenter : MonoBehaviour, IResetable
    {
        [Header("Source")]
        [SerializeField] private Slider _audioSlider;

        [Header("View")]
        [SerializeField] private AudioSettingViewer _audioSettingViewer; 

        [Header("Audio Name")]
        [SerializeField] private string _audioGroupName;

        private ResetService _resetService;

        private IAudioMixerService _audioMixerService;

        private void Start()
        {
            float value = _audioMixerService.GetFloat(_audioGroupName);

            _audioSlider.value = value;

            SetAudioValue(value);
        }

        private void OnEnable()
        {
            _resetService.Register(this);

            _audioSlider.onValueChanged.AddListener(SetAudioValue);
        }

        private void OnDisable()
        {
            _resetService.Unregister(this);

            _audioSlider.onValueChanged.RemoveListener(SetAudioValue);
        }

        [Inject]
        public void Construct(IAudioMixerService audioMixerService, ResetService resetService)
        {
            _audioMixerService = audioMixerService;

            _resetService = resetService;
        }

        private void SetAudioValue(float value)
        {
            _audioMixerService.SetFloat(_audioGroupName, value);

            RefreshUI();
        }

        private void RefreshUI()
        {
            _audioSettingViewer.RefreshUI(_audioSlider.value, _audioSlider.minValue, _audioSlider.maxValue);
        }

        void IResetable.Reset()
        {
            _audioMixerService.ResetValue(_audioGroupName, 0f);

            _audioSlider.value = 0f;

            RefreshUI();
        }
    }
}