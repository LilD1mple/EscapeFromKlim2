using EFK2.Game.PauseSystem;
using EFK2.Game.SceneService;
using EFK2.Game.SceneService.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace EFK2.Settings
{
	public class GameExitService : MonoBehaviour
	{
		[SerializeField] private Button _exitButton;

		private PauseService _pauseService;

		private ISceneLoaderService _sceneLoaderService;

		private void OnEnable()
		{
			_exitButton.onClick.AddListener(OnButtonClicked);
		}

		private void OnDisable()
		{
			_exitButton.onClick.RemoveListener(OnButtonClicked);
		}

		[Inject]
		public void Construct(ISceneLoaderService sceneLoaderService, PauseService pauseService)
		{
			_sceneLoaderService = sceneLoaderService;

			_pauseService = pauseService;
		}

		private void OnButtonClicked()
		{
			_pauseService.SetPause(false);

			_sceneLoaderService.LoadScene(SceneNames.MainMenu);
		}
	}
}