using EFK2.AI.Interfaces;
using EFK2.Game.PauseSystem;
using UnityEngine;
using Zenject;

namespace EFK2.AI
{
	public class NavigationAnimatorService : MonoBehaviour, IPauseable, INavigationAnimatorService
	{
		[SerializeField] private Animator _animator;

		[Inject] private readonly PauseService _pauseService;

		public void EnableAnimator()
		{
			_pauseService.Register(this);
		}

		public void DisableAnimator()
		{
			_pauseService.Unregister(this);
		}

		public void SetFloat(float value, int hash)
		{
			_animator.SetFloat(hash, value);
		}

		public void SetBool(bool value, int hash)
		{
			_animator.SetBool(hash, value);
		}

		public void SetTrigger(int hash)
		{
			_animator.SetTrigger(hash);
		}

		public void PlayAnimation(int hash)
		{
			_animator.Play(hash);
		}

		public void SetPause(bool isPaused)
		{
			_animator.speed = isPaused ? 0f : 1f;
		}
	}
}