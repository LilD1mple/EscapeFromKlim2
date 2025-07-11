using EFK2.Events;
using EFK2.Events.Interfaces;
using EFK2.Events.Signals;
using EFK2.Inputs;
using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace EFK2.UI.View
{
	public class HandItemGUIView : MonoBehaviour, IEventReceiver<InputKeyChangedSignal>
	{
		[Header("Source")]
		[SerializeField] private Canvas _renderCanvas;
		[SerializeField] private Camera _playerCamera;

		[Header("Transforms")]
		[SerializeField] private Transform _pointView;
		[SerializeField] private Transform _player;

		[Header("Prefabs")]
		[SerializeField] private TMP_Text _viewTextPrefab;
		[SerializeField] private GameObject _viewVFX;

		[Header("Constants")]
		[SerializeField, Range(0.1f, 20)] private float _maxTextViewRange = 3f;
		[SerializeField, Min(0f)] private float _viewVFXMaxDistance;
		[SerializeField, Min(0)] private float _minViewScale;
		[SerializeField, Min(0)] private float _maxViewScale;
		[SerializeField] private Vector3 _viewOffset;

		[Inject] private readonly EventBus _eventBus;

		private float _currentDistance;

		private TMP_Text _currentText;

		public TMP_Text CurrentText => _currentText;

		UniqueId IBaseEventReceiver.Id => new();

		private void Awake()
		{
			TMP_Text text = Instantiate(_viewTextPrefab, _renderCanvas.transform);

			_currentText = text;
		}

		private void OnEnable()
		{
			_eventBus.Subscribe(this);
		}

		private void OnDisable()
		{
			_eventBus.Unsubscribe(this);
		}

		private void FixedUpdate()
		{
			_currentText.transform.position = _playerCamera.WorldToScreenPoint(CalculateWorldPosition(_pointView.transform.position, _playerCamera) + _viewOffset);

			_currentDistance = Vector3.Distance(_player.position, _pointView.position);

			SetTextOpacity(_currentDistance < _maxTextViewRange ? 1f : 0f);

			SetViewScale(_currentDistance);
		}

		public void SetActive(bool active)
		{
			CurrentText.enabled = active;

			_viewVFX.SetActive(active);
		}

		private Vector3 CalculateWorldPosition(Vector3 position, Camera camera)
		{
			Vector3 camNormal = camera.transform.forward;

			Vector3 vectorFromCam = position - camera.transform.position;

			float camNormDot = Vector3.Dot(camNormal, vectorFromCam.normalized);

			if (camNormDot <= 0f)
			{
				float camDot = Vector3.Dot(camNormal, vectorFromCam);
				Vector3 proj = camNormal * camDot * 1.01f;
				position = camera.transform.position + (vectorFromCam - proj);
			}

			return position;
		}

		private void SetViewScale(float currentDistance)
		{
			float normalizedDistance = Mathf.Clamp01(currentDistance / _viewVFXMaxDistance);

			float scale = Mathf.Lerp(_minViewScale, _maxViewScale, normalizedDistance);

			_viewVFX.transform.localScale = new Vector3(scale, scale, scale);
		}

		private void SetTextOpacity(float opacity)
		{
			Color opacityColor = _currentText.color;

			opacityColor.a = Mathf.Lerp(opacityColor.a, opacity, 10 * Time.deltaTime);

			_currentText.color = opacityColor;
		}

		void IEventReceiver<InputKeyChangedSignal>.OnEvent(InputKeyChangedSignal @event)
		{
			if (@event.Key == InputConstants.interactKeyConst)
				_currentText.text = $"Нажмите {@event.KeyCode}, чтобы взять предмет";
		}
	}
}