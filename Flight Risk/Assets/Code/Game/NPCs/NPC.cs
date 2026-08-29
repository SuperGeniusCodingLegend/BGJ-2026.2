using UnityEngine;

namespace FlightRisk.Game.NPCs
{
    public abstract class NPC : MonoBehaviour
    {
        [SerializeField] protected Transform dialogFocusPoint;
    }
}
