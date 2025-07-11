using UnityEngine;
using UnityEngine.UI;

namespace EFK2.Game
{
	public class GameExiter : MonoBehaviour
	{
		[SerializeField] private Button _exitButton;

		private void OnEnable()
		{
			_exitButton.onClick.AddListener(Exit);
		}

		private void OnDisable()
		{
			_exitButton.onClick.RemoveListener(Exit);
		}

		private void Exit()
		{
			Application.Quit();
		}
	}
}