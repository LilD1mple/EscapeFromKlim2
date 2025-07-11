using EFK2.DialogSystem.Interfaces;
using UnityEngine;

namespace EFK2.DialogSystem.Audio
{
	public sealed class DialogAudioService : IDialogAudioService
	{
		private readonly AudioSource _audioSource;

		public DialogAudioService(AudioSource audioSource)
		{
			_audioSource = audioSource;
		}

		public void PlayClip(AudioClip clip)
		{
			_audioSource.clip = clip;

			_audioSource.Play();
		}

		public void StopClip()
		{
			_audioSource.Stop();
		}

		public void Pause()
		{
			_audioSource.Pause();
		}

		public void UnPause()
		{
			_audioSource.UnPause();
		}
	}
}