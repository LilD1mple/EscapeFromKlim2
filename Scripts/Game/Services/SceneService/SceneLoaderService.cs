using Cysharp.Threading.Tasks;
using EFK2.Game.SceneService.Interfaces;
using Michsky.LSS;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EFK2.Game.SceneService
{
	public class SceneLoaderService : MonoBehaviour, ISceneLoaderService
	{
		[SerializeField] private LoadingScreenManager _loadingScreen;

		public void LoadScene(string sceneName)
		{
			LoadSceneInternal(sceneName);
		}

		public void LoadScene(string sceneName, float delay)
		{
			WaitForDelay(delay, sceneName).Forget();
		}

		public void LoadSceneLegacy(string sceneName)
		{
			LoadSceneLegacyInternal(sceneName);
		}

		public void SetEnablePressKey(bool enable)
		{
			_loadingScreen.SetPressKeyEnable(enable);
		}

		public string GetActiveScene()
		{
			return SceneManager.GetActiveScene().name;
		}

		private async UniTaskVoid WaitForDelay(float delay, string sceneName)
		{
			if (delay > 0f)
				await UniTask.Delay(TimeSpan.FromSeconds(delay));

			LoadSceneInternal(sceneName);
		}

		private void LoadSceneInternal(string sceneName)
		{
			SceneManager.LoadScene(sceneName);
		}

		private void LoadSceneLegacyInternal(string sceneName)
		{
			_loadingScreen.LoadScene(sceneName);
		}
	}
}