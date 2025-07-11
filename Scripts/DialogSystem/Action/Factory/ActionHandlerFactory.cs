using EFK2.DialogSystem.Scenes;
using System;
using UnityEngine.UI;

namespace EFK2.DialogSystem.Actions.Factory
{
	public sealed class ActionHandlerFactory : IActionHandlerFactory
	{
		private readonly Image _personImage;

		public ActionHandlerFactory(Image personImage)
		{
			_personImage = personImage;
		}

		public IActionHandler GetActionHandler(PersonActions personActions, in SentenceActions sentenceActions, in Sentence currentSentence) => personActions switch
		{
			PersonActions.Appearance => new AppearanceActionHandler(_personImage, sentenceActions.duration),
			PersonActions.Disappearance => new DisappearanceActionHandler(_personImage, sentenceActions.duration),
			PersonActions.SpriteChange => new SpriteChangeActionHandler(_personImage, currentSentence.speaker.SpeakerSprite),
			_ => throw new ArgumentOutOfRangeException(nameof(sentenceActions)),
		};
	}
}
