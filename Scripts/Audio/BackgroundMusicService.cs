using DG.Tweening;
using EFK2.Audio.Interfaces;
using EFK2.Extensions;
using System;
using UnityEngine;

namespace EFK2.Audio
{
    public class BackgroundMusicService : MonoBehaviour, IMusicService
    {
        [SerializeField] private AudioSource _audioSource;

        private Tween _currentTween;

        private void ResetMusic(float duration)
        {
            if (duration < 0)
                throw new ArgumentOutOfRangeException(nameof(duration));

            CheckTweenKilled();

            _currentTween = _audioSource.DOFade(0f, duration).OnComplete(() =>
            {
                _audioSource.Stop();

                _audioSource.clip = null;
            });
        }

        private void CheckTweenKilled()
        {
            if (_currentTween.IsActive())
                _currentTween.Kill(true);
        }

        void IMusicService.ResetMusic(float duration)
        {
            ResetMusic(duration);
        }

        void IMusicService.SetBackgroundMusic(AudioClip clip)
        {
            CheckTweenKilled();

            _currentTween = _audioSource.DOFade(1f, 0f);

            _audioSource.PlayAudio(clip);
        }

        void IMusicService.SetMuted(bool mute)
        {
            _audioSource.mute = mute;
        }

        void IMusicService.SetLoop(bool loop)
        {
            _audioSource.loop = loop;
        }
    }
}