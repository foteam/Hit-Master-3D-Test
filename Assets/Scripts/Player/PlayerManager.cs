using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Player
{
    public class PlayerManager : MonoBehaviour 
    {
        private Dictionary<Type, PlayerState> _states = new Dictionary<Type, PlayerState>();
        private PlayerState CurrentState { get; set; }

        public void AddNewState(PlayerState state)
        {
            _states.Add(state.GetType(), state);
        }
        public void SetState<T>(params object[] args) where T : PlayerState
        {
            var type = typeof(T);
            if (CurrentState != null && CurrentState.GetType() == type)
            {
                return;
            }

            if (_states.TryGetValue(type, out var newState))
            {
                CurrentState?.Exit();
                CurrentState = newState;
                CurrentState.Enter();
            }
        }
        public void Update()
        {
            CurrentState?.Update();
        }
    }
}