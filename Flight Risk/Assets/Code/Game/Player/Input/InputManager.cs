using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using FlightRisk.Game.Player.Input;

namespace FlightRisk.Game.Player
{
    public class InputManager : MonoBehaviour , IServiceProvider<InputManager>
    {
        public InputAxis Move;
        public InputAxis Look;
        public InputButton Primary; // Give Item, Fire etc
        public InputButton Secondary; // Grab Item, Talk etc
        // Other buttons go here

        public void GetMoveInput(InputAction.CallbackContext ctx) => HandleCallback(Move, ctx);
        public void GetLookInput(InputAction.CallbackContext ctx) => HandleCallback(Look, ctx);
        public void GetPrimaryInput(InputAction.CallbackContext ctx) => HandleCallback(Primary, ctx);
        public void GetSecondaryInput(InputAction.CallbackContext ctx) => HandleCallback(Secondary, ctx);
        // Other gets for buttons go here

        private void Awake()
        {
            this.InjectService(this);
        }

        private void HandleCallback(GameInput input, InputAction.CallbackContext ctx)
        {
            input.HandleCallback(ctx);
            if (input.CurrentState == GameInput.State.Released) this.DelayByFrame(() => ReleaseToIdle(input));
        }

        private void ReleaseToIdle(GameInput input)
        {
            if (input.CurrentState == GameInput.State.Released) input.SetState(GameInput.State.Idle);
        }
    }
}