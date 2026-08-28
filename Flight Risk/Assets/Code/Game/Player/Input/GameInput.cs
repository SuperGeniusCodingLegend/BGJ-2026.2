using UnityEngine.InputSystem;

namespace FlightRisk.Game.Player.Input
{
    [System.Serializable]
    public abstract class GameInput
    {
        public enum State { Idle, Pressed, Held, Released }

        public State CurrentState;

        public virtual bool IsActuated() => CurrentState == State.Pressed || CurrentState == State.Held;
        public virtual bool WasActuatedThisFrame() => CurrentState == State.Pressed;

        public virtual void SetState(State newState)
        {
            CurrentState = newState;
        }

        public virtual void HandleCallback(InputAction.CallbackContext ctx)
        {
            SetState(ParseStateFromContext(ctx));
        }

        private State ParseStateFromContext(InputAction.CallbackContext ctx)
        {
            return ctx switch
            {
                _ when ctx.started => State.Pressed,
                _ when ctx.performed => State.Held,
                _ when ctx.canceled => State.Released,
                _ => State.Idle
            };
        }
    }
}
