using EFK2.Audio.Interfaces;
using EFK2.Game.Save;
using UnityEngine;
using UnityEngine.Audio;

namespace EFK2.Audio
{
    public class AudioMixerService : MonoBehaviour, IAudioMixerService
    {
        [SerializeField] private AudioMixer _audioMixer;

        private const float _minAudioMixerValue = -80f;
        private const float _maxAudioMixerValue = 10f;

        private void AssignNewValue(string name, float value)
        {
            float clampedValue = Mathf.Clamp(value, _minAudioMixerValue, _maxAudioMixerValue);

            _audioMixer.SetFloat(name, clampedValue);

            SaveUtility.SaveData(name, clampedValue);
        }

        void IAudioMixerService.SetFloat(string name, float value)
        {
            AssignNewValue(name, value);
        }

        void IAudioMixerService.ResetValue(string name, float defaultValue)
        {
            SaveUtility.DeleteKey(name);

            AssignNewValue(name, defaultValue);
        }

        float IAudioMixerService.GetFloat(string name)
        {
            return SaveUtility.LoadData(name, 0f);
        }
    }
}