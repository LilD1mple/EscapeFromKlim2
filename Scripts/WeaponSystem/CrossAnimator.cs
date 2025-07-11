using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine;

namespace EFK2.WeaponSystem
{
	public class CrossAnimator : MonoBehaviour
	{
		[Header("Source")]
		[SerializeField] private Transform _crossTransform;

		[Header("Settings")]
		[SerializeField] private float _attackHeight = 1f;
		[SerializeField] private float _animationDuration = 0.5f;
		[SerializeField] private Ease _attackEase;

		[Header("Rotation")]
		[SerializeField] private Vector3 _targetRotation;
		[SerializeField] private Vector3 _previousRotation;

		public void PlayCrossAttackAnimation(Action callback)
		{
			PlayAnimation(callback).Forget();
		}

		private async UniTaskVoid PlayAnimation(Action callback)
		{
			UniTask crossDownwardMotionAnimation = _crossTransform.DOLocalMoveY(Vector3.zero.y - _attackHeight, _animationDuration / 2).SetEase(_attackEase).ToUniTask();
			UniTask crossDownwardRotationAnimation = _crossTransform.DOLocalRotate(_targetRotation, _animationDuration / 2).SetEase(_attackEase).ToUniTask();

			await UniTask.WhenAll(crossDownwardMotionAnimation, crossDownwardRotationAnimation);

			UniTask crossUpwardMotionAnimation = _crossTransform.DOLocalMoveY(Vector3.zero.y, _animationDuration / 2).SetEase(_attackEase).ToUniTask();
			UniTask crossUpwardRotationAnimation = _crossTransform.DOLocalRotate(_previousRotation, _animationDuration / 2).SetEase(_attackEase).ToUniTask();

			await UniTask.WhenAll(crossUpwardMotionAnimation, crossUpwardRotationAnimation);

			callback?.Invoke();
		}
	}
}
