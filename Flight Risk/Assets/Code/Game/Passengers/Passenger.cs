using UnityEngine;

namespace FlightRisk.Game.NPCs
{
    public class Passenger : NPC
    {
        public Transform TaskParent => taskParent;

        [SerializeField] private Transform taskParent;
    }
}
