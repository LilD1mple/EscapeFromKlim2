namespace EFK2.Game.SceneService.Interfaces
{
	public interface ISceneLoaderService
	{
		void LoadScene(string sceneName);

		void LoadScene(string sceneName, float delay);

		void LoadSceneLegacy(string sceneName);

		void SetEnablePressKey(bool enable);

		string GetActiveScene();
	}
}
