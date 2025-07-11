using UnityEngine;

namespace EFK2.DialogSystem.Interfaces
{
	public interface IDialogAudioService
	{
		void PlayClip(AudioClip clip);
		void StopClip();
		void Pause();
		void UnPause();
	}
}