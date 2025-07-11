using EFK2.Audio.Interfaces;
using EFK2.Difficult;
using EFK2.Game.Save.Progress;
using EFK2.Game.SceneService.Interfaces;
using ModestTree;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace EFK2.Game.Save
{
	public class ProgressLoaderPresenter : MonoBehaviour
	{
		[Header("Template")]
		[SerializeField] private SavedProgressRow _progressRowPrefab;

		[Header("Source")]
		[SerializeField] private Button _progressWindowCloseButton;
		[SerializeField] private Transform _content;

		[Header("Sprites")]
		[SerializeField] private List<Sprite> _progressSprites;

		[Header("Events")]
		[SerializeField] private UnityEvent _progressNotFoundCallback;
		[SerializeField] private UnityEvent _progressLoad;
		[SerializeField] private UnityEvent _confirmDelete;
		[SerializeField] private UnityEvent _deleteConfirm;

		private Dictionary<string, Sprite> _progressSpritesKeys;

		private Action _deleteAction;
		private ProgressData _currentDeleteData;

		private ISceneLoaderService _sceneLoaderService;
		private IProgressService _progressService;
		private IDifficultService _difficultService;
		private IMusicService _musicService;

		private void OnEnable()
		{
			_progressWindowCloseButton.onClick.AddListener(OnProgressWindowClosed);
		}

		private void OnDisable()
		{
			_progressWindowCloseButton.onClick.RemoveListener(OnProgressWindowClosed);
		}

		[Inject]
		public void Construct(ISceneLoaderService sceneLoaderService, IProgressService progressService, IDifficultService difficultService, IMusicService musicService)
		{
			_progressService = progressService;

			_sceneLoaderService = sceneLoaderService;

			_difficultService = difficultService;

			_musicService = musicService;

			CreateDictionary();
		}

		public void DrawProgresses()
		{
			LoadProgresses();
		}

		private void CreateDictionary()
		{
			_progressSpritesKeys = new();

			for (int i = 0; i < _progressSprites.Count; i++)
			{
				_progressSpritesKeys.Add(_progressSprites[i].name, _progressSprites[i]);
			}
		}

		private void LoadProgresses()
		{
			IReadOnlyCollection<ProgressData> progresses = _progressService.GetProgresses();

			if (progresses.IsEmpty())
			{
				_progressNotFoundCallback?.Invoke();

				return;
			}

			foreach (ProgressData progressData in progresses)
			{
				SavedProgressRow progressRow = Instantiate(_progressRowPrefab, _content);

				progressRow.Construct(this, progressData, _progressSpritesKeys[progressData.progressScreenName]);
			}

			_progressLoad?.Invoke();
		}

		public void LoadProgress(ProgressData progressData)
		{
			_difficultService.SetLevelDifficult(progressData.difficultLevel);

			_musicService.ResetMusic(2f);

			_sceneLoaderService.LoadSceneLegacy(progressData.sceneName);
		}

		public void RemoveProgress(Action onDelete, ProgressData progressData)
		{
			_currentDeleteData = progressData;

			_deleteAction = onDelete;

			_confirmDelete?.Invoke();
		}

		public void OnConfirmDeleteButtonClicked()
		{
			_progressService.RemoveProgress(_currentDeleteData);

			_deleteConfirm?.Invoke();

			_deleteAction?.Invoke();
		}

		private void OnProgressWindowClosed()
		{
			for (int i = 0; i < _content.childCount; i++)
			{
				Destroy(_content.GetChild(i).gameObject);
			}
		}
	}
}