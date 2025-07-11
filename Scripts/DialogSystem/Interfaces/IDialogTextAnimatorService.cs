using EFK2.DialogSystem.Scenes;

namespace EFK2.DialogSystem.Interfaces
{
	public interface IDialogTextAnimatorService
	{
		bool IsPlaying { get; }

		void AnimateSentence(ref Sentence sentence);
		void SetPause(bool isPaused);
		void ClearText();
	}
}