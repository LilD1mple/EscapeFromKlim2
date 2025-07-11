using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace NTC.ContextStateMachine
{
	public sealed class StateMachineStateSwitcher : IStateMachineStateSwitcher
	{
		private int _currentStateIndex = 0;

		private bool _isRunning = false;

		private readonly List<SwitchableState> _switchableStates = new();

		private readonly StateMachine _stateMachine;
		private readonly MonoBehaviour _context;

		private CancellationTokenSource _cancellationTokenSource;

		public StateMachineStateSwitcher(StateMachine stateMachine, MonoBehaviour context)
		{
			_stateMachine = stateMachine;
			_context = context;
		}

		public IStateMachineStateSwitcher AppendState(State state, Func<bool> exitPredicate)
		{
			_switchableStates.Add(new SwitchableState() { state = state, exitPredicate = exitPredicate, isSkippable = false, delay = 0f });

			return this;
		}

		public IStateMachineStateSwitcher AppendState(State state, float delay)
		{
			_switchableStates.Add(new SwitchableState() { state = state, delay = delay, isSkippable = true, exitPredicate = null });

			return this;
		}

		public void Start()
		{
			if (_isRunning || _context.gameObject.activeSelf == false)
				return;

			_isRunning = true;

			_cancellationTokenSource ??= new CancellationTokenSource();

			SwitchStateLoop().Forget();
		}

		public void Stop()
		{
			if (_isRunning == false)
				return;

			_cancellationTokenSource?.Cancel();
			_cancellationTokenSource?.Dispose();

			_cancellationTokenSource = null;

			_isRunning = false;
		}

		private async UniTaskVoid SwitchStateLoop()
		{
			while (_isRunning)
			{
				if (_isRunning == false)
					break;

				_currentStateIndex++;

				if (_currentStateIndex >= _switchableStates.Count)
					_currentStateIndex = 0;

				SwitchableState switchableState = _switchableStates[_currentStateIndex];

				_stateMachine.SetState(switchableState.state);

				if (switchableState.isSkippable)
				{
					await UniTask.Delay(TimeSpan.FromSeconds(switchableState.delay), cancellationToken: _cancellationTokenSource.Token);
				}
				else
				{
					await UniTask.WaitUntil(switchableState.exitPredicate, cancellationToken: _cancellationTokenSource.Token);
				}
			}
		}
	}

	public struct SwitchableState
	{
		public State state;
		public bool isSkippable;
		public float delay;

		public Func<bool> exitPredicate;
	}
}
