using EFK2.Audio.Interfaces;
using EFK2.Game.SceneService;
using EFK2.Game.SceneService.Interfaces;
using UnityEngine;
using Zenject;

namespace EFK2.UI
{
	public class LibrarySceneLoader : MonoBehaviour
	{
		private ISceneLoaderService _sceneLoaderService;

		private IMusicService _musicService;

		[Inject]
		public void Construct(ISceneLoaderService sceneLoaderService, IMusicService musicService)
		{
			_musicService = musicService;

			_sceneLoaderService = sceneLoaderService;
		}

		public void LoadLibrary()
		{
			_musicService.ResetMusic(2f);

			_sceneLoaderService.LoadSceneLegacy(SceneNames.Introduction);
		}
	}
}