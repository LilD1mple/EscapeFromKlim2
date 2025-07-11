using EFK2.DialogSystem.Scenes;

namespace EFK2.DialogSystem.Interfaces
{
	public interface IDialogScenePlayer
	{
		bool IsFirstSentence { get; }
		bool IsLastSentence { get; }
		bool IsPlaying { get; }

		void SetDialogScene(DialogScene dialogScene);
		void PlayNextSentence();
		void PlayScene(DialogScene storyScene);
		void ResetSentenceIndex();
	}
}