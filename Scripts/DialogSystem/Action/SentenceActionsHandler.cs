using Cysharp.Threading.Tasks;
using EFK2.DialogSystem.Actions.Factory;
using EFK2.DialogSystem.Interfaces;
using EFK2.DialogSystem.Scenes;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace EFK2.DialogSystem.Actions
{
	public sealed class SentenceActionsHandler : ISentenceActionsHandler
	{
		private readonly IActionHandlerFactory _actionHandlerFactory;

		public SentenceActionsHandler(Image personImage)
		{
			_actionHandlerFactory = new ActionHandlerFactory(personImage);
		}

		public void HandleActions(ref Sentence currentSentence)
		{
			HandleSentenceActions(currentSentence).Forget();
		}

		private async UniTaskVoid HandleSentenceActions(Sentence currentSentence)
		{
			List<SentenceActions> sentenceActions = currentSentence.sentenceActions;

			for (int i = 0; i < sentenceActions.Count; i++)
			{
				SentenceActions action = sentenceActions[i];

				IActionHandler actionHandler = _actionHandlerFactory.GetActionHandler(action.personAction, action, currentSentence);

				actionHandler.Handle();

				await UniTask.Delay(TimeSpan.FromSeconds(action.duration));
			}
		}
	}
}
