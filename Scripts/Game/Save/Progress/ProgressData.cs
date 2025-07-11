using EFK2.Difficult;
using System;

namespace EFK2.Game.Save.Progress
{
	[Serializable]
	public struct ProgressData
	{
		public string title;
		public string date;
		public string sceneName;
		public string progress;
		public string progressScreenName;
		public DifficultLevelConfiguration difficultLevel;
	}
}
