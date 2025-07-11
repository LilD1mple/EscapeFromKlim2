using System;

namespace NTC.ContextStateMachine
{
	public interface IStateMachineStateSwitcher
	{
		IStateMachineStateSwitcher AppendState(State state, float delay);
		IStateMachineStateSwitcher AppendState(State state, Func<bool> exitPredicate);
		void Start();
		void Stop();
	}
}