using EFK2.DialogSystem.Scenes;

namespace EFK2.DialogSystem.Actions.Factory
{
	public interface IActionHandlerFactory
	{
		IActionHandler GetActionHandler(PersonActions personActions, in SentenceActions sentenceActions, in Sentence currentSentence);
	}
}
