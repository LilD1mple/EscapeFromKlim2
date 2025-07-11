using EFK2.Target.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace EFK2.Player.Aim
{
	public class AimPresenter : MonoBehaviour, IAimService
	{
		[Header("Main")]
		[SerializeField] private Image _crosshair;
		[SerializeField] private GameObject _targetMarker;

		[Header("Settngs")]
		[SerializeField] private LayerMask _layerMask;
		[SerializeField] private float _preCastDistance;

		private Camera _mainCamera;

		private bool TargetMarkerEnabled => _targetMarker.activeSelf;

		[Inject]
		public void Construct(ITargetService playerTarget)
		{
			_mainCamera = playerTarget.PlayerController.MainCamera;
		}

		private void Update()
		{
			if (TargetMarkerEnabled)
				UpdateTargetMarkerPosition();
		}

		private void UpdateTargetMarkerPosition()
		{
			Ray ray = new Ray(_mainCamera.transform.position, _mainCamera.transform.forward.normalized);

			if (Physics.Raycast(ray, out var hit, _preCastDistance, _layerMask))
				_targetMarker.transform.SetPositionAndRotation(hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
		}

		void IAimService.SetTargetAim(AimType aimType)
		{
			bool enable = aimType == AimType.TargetMarker;

			_targetMarker.SetActive(enable);

			_crosshair.enabled = enable == false;
		}
	}
}