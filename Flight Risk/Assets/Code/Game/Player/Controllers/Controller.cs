using UnityEngine;

namespace FlightRisk.Game.Player
{
    public abstract class Controller : MonoBehaviour
    {
        protected InputManager input;

        public virtual void Setup(InputManager playerInput)
        {
            input = playerInput;
        }
    }
}