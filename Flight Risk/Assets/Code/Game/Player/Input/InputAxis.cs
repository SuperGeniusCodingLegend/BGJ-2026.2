using UnityEngine;
using UnityEngine.InputSystem;

namespace FlightRisk.Game.Player.Input
{
    [System.Serializable]  
    public class InputAxis : GameInput
    {
        public Vector2 CurrentAxis;

        public override void HandleCallback(InputAction.CallbackContext ctx)
        {
            base.HandleCallback(ctx);
            CurrentAxis = ctx.ReadValue<Vector2>();
        }
    }
}