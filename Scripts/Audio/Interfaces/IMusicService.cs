using UnityEngine;

namespace EFK2.Audio.Interfaces
{
    public interface IMusicService
    {
        void SetBackgroundMusic(AudioClip clip);

        void SetMuted(bool mute);

        void SetLoop(bool loop);

        void ResetMusic(float duration);
    }
}