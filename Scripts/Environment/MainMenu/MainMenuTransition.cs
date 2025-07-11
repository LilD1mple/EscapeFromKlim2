using EFK2.Game.SceneService;
using EFK2.Game.SceneService.Interfaces;
using UnityEngine;
using Zenject;

namespace EFK2.Environment
{
	public class MainMenuTransition : MonoBehaviour
	{
		[Inject] private readonly ISceneLoaderService _sceneLoaderService;

		public void StartTransition()
		{
			_sceneLoaderService.LoadScene(SceneNames.MainMenu);
		}
	}
}