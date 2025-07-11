using EFK2.Audio.Interfaces;
using UnityEngine;
using Zenject;

namespace EFK2.Environment
{
    public class MainMenuMusicPresenter : MonoBehaviour
    {
        [SerializeField] private AudioClip _backgroundAmbientClip;

        [Inject] private readonly IMusicService _musicService;

        private void Awake()
        {
            _musicService.SetBackgroundMusic(_backgroundAmbientClip);
        }
    }
}