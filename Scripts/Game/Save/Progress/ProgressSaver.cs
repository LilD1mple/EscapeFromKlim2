using UnityEngine;
using Zenject;

namespace EFK2.Game.Save
{
	public class ProgressSaver : MonoBehaviour
	{
		[Header("Source")]
		[SerializeField] private string _sceneName;
		[SerializeField] private string _progress;
		[SerializeField] private Sprite _progressSprite;

		[Inject] private readonly IProgressService _progressService;

		public void SaveProgress()
		{
			_progressService.AddProgress(_sceneName, _progress, _progressSprite.name);
		}
	}
}