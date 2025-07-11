using DG.Tweening;
using UnityEngine;

namespace EFK2.Environment
{
	public class AudioSourceFader : MonoBehaviour
	{
		[Header("Source")]
		[SerializeField] private AudioSource _audioSource;

		[Header("Settings")]
		[SerializeField] private float _duration;

		private const float EndValue = 0f;

		public void FadeAudio()
		{
			_audioSource.DOFade(EndValue, _duration).SetRecyclable(true);
		}
	}
}