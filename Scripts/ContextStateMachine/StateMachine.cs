﻿using System;
using System.Collections.Generic;

namespace NTC.ContextStateMachine
{
    public sealed class StateMachine
    {
        public event Action<State> StateChanged;

        public bool TransitionsEnabled { get; set; } = true;
        public bool HasCurrentState { get; private set; }
        
        public State CurrentState { get; private set; }
        public Transition CurrentTransition { get; private set; }

        private readonly List<Transition> _anyTransitions = new(16);
        private readonly List<Transition> _transitions = new(16);

        public void SetState(State state)
        {
            if (state == null)
                throw new NullReferenceException(nameof(state), null);
            
            if (state == CurrentState)
                return;

            if (HasCurrentState)
            {
                DisposeCurrentState();
            }
            
            CurrentState = state;
            HasCurrentState = true;
            
            PrepareCurrentState();

            StateChanged?.Invoke(state);
        }
        
        public void AddTransition(State from, State to, Func<bool> condition)
        {
            _transitions.Add(new Transition(from, to, condition));
        }

        public void AddAnyTransition(State to, Func<bool> condition)
        {
            _anyTransitions.Add(new Transition(null, to, condition));
        }
        
        public void Run()
        {
            if (TransitionsEnabled)
            {
                SetStateByTransitions();
            }

            if (HasCurrentState)
            {
                CurrentState.OnRun();
            }
        }

        public void FixedRun()
        {
            if (HasCurrentState)
                CurrentState.OnFixedRun();
        }

        public void SetStateByTransitions()
        {
            CurrentTransition = GetTransition();

            if (CurrentTransition != null)
            {
                SetState(CurrentTransition.To);
            }   
        }
        
        private void PrepareCurrentState()
        {
            CurrentState.IsActive = true;
            CurrentState.OnEnter();
        }

        private void DisposeCurrentState()
        {
            CurrentState.IsActive = false;
            CurrentState.OnExit();
        }

        private Transition GetTransition()
        {
            for (var i = 0; i < _anyTransitions.Count; i++)
            {
                if (_anyTransitions[i].Condition())
                {
                    return _anyTransitions[i];
                }
            }

            for (var i = 0; i < _transitions.Count; i++)
            {
                if (_transitions[i].From.IsActive == false)
                {
                    continue;
                }
                
                if (_transitions[i].Condition())
                {
                    return _transitions[i];
                }
            }

            return default;
        }
    }
}