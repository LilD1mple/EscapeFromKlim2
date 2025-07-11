using EFK2.Audio.Interfaces;
using UnityEngine;
using Zenject;

namespace EFK2.Environment
{
	public class CreditsBackgroundMusicPresenter : MonoBehaviour
	{
		[Header("Music")]
		[SerializeField] private AudioClip _backgroundMusic;

		[Inject] private readonly IMusicService _musicService;

		private const float ResetDuration = 0f;

		private void Start()
		{
			_musicService.SetBackgroundMusic(_backgroundMusic);
		}

		public void ResetMusic()
		{
			_musicService.ResetMusic(ResetDuration);
		}
	}
}