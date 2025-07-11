using UnityEngine;
using UnityEngine.UI;

namespace EFK2.DialogSystem.Actions
{
	public sealed class SpriteChangeActionHandler : IActionHandler
	{
		private readonly Image _personImage;

		private readonly Sprite _newSprite;

		public SpriteChangeActionHandler(Image personImage, Sprite newSprite)
		{
			_personImage = personImage;
			_newSprite = newSprite;
		}

		public void Handle()
		{
			_personImage.sprite = _newSprite;

			_personImage.SetNativeSize();
		}
	}
}
