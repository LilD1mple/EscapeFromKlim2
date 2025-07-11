using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFK2.Audio
{
	public class MusicPlayerView : MonoBehaviour
	{
		[Header("Source")]
		[SerializeField] private Image _separatorMusic;
		[SerializeField] private TMP_Text _nowPlayingText;
		[SerializeField] private TMP_Text _musicNameText;

		[Header("Constants")]
		[SerializeField] private float _fillDuration;
		[SerializeField] private float _duration;
		[SerializeField] private float _interval;

		private const float ShowedNowPlayingTextAlpha = 1f;
		private const float ShowedCurrentMusicNameTextAlpha = 1f;
		private const float FilledMusicSeparatorValue = 1f;
		private const float HidedNowPlayingTextAlpha = 0f;
		private const float HidedCurrentMusicNameTextAlpha = 0f;
		private const float UnfilledMusicSeparatorValue = 0f;

		public void ViewMusic(AudioClip clip)
		{
			SetMusicName(clip.name);

			PlayAnimation();
		}

		private void SetMusicName(string name)
		{
			_musicNameText.text = name;
		}

		private void PlayAnimation()
		{
			DOTween.Sequence()
				.Append(_nowPlayingText.DOFade(ShowedNowPlayingTextAlpha, _duration))
				.Join(_musicNameText.DOFade(ShowedCurrentMusicNameTextAlpha, _duration))
				.Join(_separatorMusic.DOFillAmount(FilledMusicSeparatorValue, _fillDuration))
				.AppendInterval(_interval)
				.Append(_nowPlayingText.DOFade(HidedNowPlayingTextAlpha, _duration))
				.Join(_musicNameText.DOFade(HidedCurrentMusicNameTextAlpha, _duration))
				.Join(_separatorMusic.DOFillAmount(UnfilledMusicSeparatorValue, _fillDuration));
		}
	}
}
