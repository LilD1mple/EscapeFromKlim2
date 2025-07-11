using EFK2.Game.SceneService.Interfaces;
using EFK2.Player;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace EFK2.Environment
{
	[RequireComponent(typeof(Collider))]
	public class LevelExitZone : MonoBehaviour
	{
		[Header("Source")]
		[SerializeField] private string _nextSceneName;

		[Header("Events")]
		[SerializeField] private UnityEvent _locationComplete;

		[Inject] private readonly ISceneLoaderService _sceneLoaderService;

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out Unit unit))
			{
				_locationComplete?.Invoke();

				_sceneLoaderService.SetEnablePressKey(false);

				_sceneLoaderService.LoadScene(_nextSceneName);
			}
		}
	}
}