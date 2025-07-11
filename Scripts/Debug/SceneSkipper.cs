using UnityEngine;
using UnityEngine.SceneManagement;

namespace EFK2.Debugs
{
	public class SceneSkipper : MonoBehaviour
	{
		[SerializeField] private string _previousScene;
		[SerializeField] private string _nextScene;

		private KeyCode _nextSceneKey = KeyCode.Alpha0;
		private KeyCode _previousKey = KeyCode.Alpha9;

		private void Update()
		{
			if (Input.GetKeyDown(_nextSceneKey))
				SceneManager.LoadScene(_nextScene);

			if (Input.GetKeyDown(_previousKey))
				SceneManager.LoadScene(_previousScene);
		}
	}
}