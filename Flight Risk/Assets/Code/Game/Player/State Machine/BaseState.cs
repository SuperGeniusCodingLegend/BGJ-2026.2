using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FlightRisk.Game.Player.States
{
    public abstract class BaseState : MonoBehaviour
    {
        public abstract PlayerState GetThisState();
        public abstract PlayerState Tick();

        [SerializeField] protected UnityEvent onStateEnter;
        [SerializeField] protected UnityEvent onStateExit;

        protected virtual void OnEnable()
        {
            onStateEnter?.Invoke();
        }

        protected virtual void OnDisable()
        {
            onStateExit?.Invoke();
        }
    }
}