using System;
using UnityEngine;

namespace FlightRisk.Game.Player
{
    public abstract class Controller : MonoBehaviour , IRequireService<InputManager>
    {
        protected InputManager input;

        protected virtual void Awake()
        {
            this.WaitForService(service => input = service);
        }
    }
}