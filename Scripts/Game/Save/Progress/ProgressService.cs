using EFK2.Difficult;
using EFK2.Game.Save.Progress;
using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace EFK2.Game.Save
{
	public sealed class ProgressService : IProgressService
	{
		private readonly IDifficultService _difficultService;

		private List<ProgressData> _progressDatas;

		private const string ProgressesSavedKey = "Progresses";
		private const string ProgressesFileName = "SavedProgresses.es3";

		[Inject]
		public ProgressService(IDifficultService difficultService)
		{
			_difficultService = difficultService;

			LoadProgresses();
		}

		private void LoadProgresses()
		{
			_progressDatas = SaveUtility.LoadData(ProgressesSavedKey, new List<ProgressData>(), ProgressesFileName);
		}

		public void AddProgress(string sceneName, string progress, string progressSpriteName)
		{
			ProgressData progressData = new()
			{
				title = $"Сохранение {_progressDatas.Count + 1}",
				sceneName = sceneName,
				date = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"),
				progress = progress,
				progressScreenName = progressSpriteName,
				difficultLevel = _difficultService.DifficultConfiguration
			};

			_progressDatas.Add(progressData);

			SaveProgresses();
		}

		public void RemoveProgress(ProgressData progressData)
		{
			_progressDatas.Remove(progressData);

			SaveProgresses();
		}

		public IReadOnlyCollection<ProgressData> GetProgresses()
		{
			return _progressDatas.AsEnumerable().Reverse().ToList();
		}

		private void SaveProgresses()
		{
			SaveUtility.SaveData(ProgressesSavedKey, _progressDatas, ProgressesFileName);
		}
	}
}