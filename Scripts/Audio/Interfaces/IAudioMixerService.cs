namespace EFK2.Audio.Interfaces
{
    public interface IAudioMixerService
    {
        float GetFloat(string name);

        void SetFloat(string name, float value);

        void ResetValue(string name, float defaultValue);
    }
}
