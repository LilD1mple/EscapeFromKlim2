using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFK2.Game.Save.Progress
{
	public class SavedProgressRow : MonoBehaviour
	{
		[Header("Source Text")]
		[SerializeField] private TMP_Text _saveTitle;
		[SerializeField] private TMP_Text _saveDescription;

		[Header("Source Image")]
		[SerializeField] private Image _saveIcon;

		[Header("Source Button")]
		[SerializeField] private Button _loadButton;
		[SerializeField] private Button _deleteButton;

		private ProgressLoaderPresenter _progressLoader;

		private ProgressData _progressData;

		private void OnEnable()
		{
			_loadButton.onClick.AddListener(OnLoadButtonClicked);

			_deleteButton.onClick.AddListener(OnDeleteButtonClicked);
		}

		private void OnDisable()
		{
			_loadButton.onClick.RemoveListener(OnLoadButtonClicked);

			_deleteButton.onClick.RemoveListener(OnDeleteButtonClicked);
		}

		public void Construct(ProgressLoaderPresenter progressLoader, ProgressData progressData, Sprite progressSprite)
		{
			_progressLoader = progressLoader;

			_progressData = progressData;

			FillFields(progressSprite);
		}

		private void FillFields(Sprite progressSprite)
		{
			_saveTitle.text = _progressData.title;

			_saveDescription.text = $"{_progressData.progress}, {_progressData.date}, {_progressData.difficultLevel.DifficultName}";

			_saveIcon.sprite = progressSprite;
		}

		private void OnLoadButtonClicked()
		{
			_progressLoader.LoadProgress(_progressData);
		}

		private void OnDeleteButtonClicked()
		{
			_progressLoader.RemoveProgress(OnDeleteConfirmed, _progressData);
		}

		private void OnDeleteConfirmed()
		{
			Destroy(gameObject);
		}
	}
}