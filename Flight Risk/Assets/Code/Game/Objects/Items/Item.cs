using UnityEngine;

namespace FlightRisk.Game
{
    public abstract class Item : MonoBehaviour
    {
        public enum State { Dropped, Held }
        public enum Type { Misc, Coffee, Snack, FireExtinguisher, }

        public abstract void TriggerHoldAction();
    }
}
