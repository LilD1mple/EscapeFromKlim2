using DG.Tweening;
using EFK2.Player;
using UnityEngine;
using UnityEngine.UI;

namespace EFK2.UI.View
{
	public class PlayerStaminaViewer : MonoBehaviour
	{
		[Header("Player")]
		[SerializeField] private PlayerController _player;

		[Header("Source")]
		[SerializeField] private Image _staminaBar;

		[Header("Settings")]
		[SerializeField, Min(0f)] private float _duration;

		private void OnEnable()
		{
			_player.StaminaChange += OnStaminaChanged;
		}

		private void OnDisable()
		{
			_player.StaminaChange -= OnStaminaChanged;
		}

		private void OnStaminaChanged(float totalStamina, float currentStamina)
		{
			_staminaBar.DOFillAmount(Mathf.InverseLerp(0, totalStamina, currentStamina), _duration / 2);
		}
	}
}