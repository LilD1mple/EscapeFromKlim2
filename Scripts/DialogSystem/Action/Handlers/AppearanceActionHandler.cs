using DG.Tweening;
using UnityEngine.UI;

namespace EFK2.DialogSystem.Actions
{
	public sealed class AppearanceActionHandler : IActionHandler
	{
		private readonly Image _personImage;

		private readonly float _duration;

		private const float EndValue = 1f;

		public AppearanceActionHandler(Image personImage, float duration)
		{
			_personImage = personImage;
			_duration = duration;
		}

		public void Handle()
		{
			_personImage.DOFade(EndValue, _duration);
		}
	}
}
