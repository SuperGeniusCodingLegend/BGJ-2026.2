using UnityEngine;

namespace FlightRisk.Game.Player.States
{
    public class InactiveState : BaseState
    {
        public override PlayerState GetThisState() => PlayerState.Inactive;

        private bool hasStarted;

        protected override void OnEnable()
        {
            base.OnEnable();
            GameEvents.TrySubscribe((uint)GameEvents.Game.Start, OnGameStart);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            GameEvents.TryUnsubscribe((uint)GameEvents.Game.Start, OnGameStart);
        }

        public override PlayerState Tick()
        {
            return hasStarted ? GetThisState() : PlayerState.Moving;
        }

        private void OnGameStart(object payload)
        {
            hasStarted = true;
        }
    }
}
