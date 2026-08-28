using UnityEngine;

namespace FlightRisk.Game
{
    public abstract class Item : MonoBehaviour
    {
        public enum State { Dropped, Held }

        public abstract void TriggerHoldAction();
    }
}
