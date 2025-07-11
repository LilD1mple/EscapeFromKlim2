using EFK2.Game.Save.Progress;
using System.Collections.Generic;

namespace EFK2.Game.Save
{
	public interface IProgressService
	{
		IReadOnlyCollection<ProgressData> GetProgresses();
		void RemoveProgress(ProgressData progressData);
		void AddProgress(string sceneName, string progress, string progressSpriteName);
	}
}