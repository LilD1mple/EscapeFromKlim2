using EFK2.DialogSystem.Scenes;

namespace EFK2.DialogSystem.Interfaces
{
	public interface ISentenceActionsHandler
	{
		void HandleActions(ref Sentence currentSentence);
	}
}