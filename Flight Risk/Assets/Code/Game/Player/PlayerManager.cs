using UnityEngine;
using FlightRisk.Game.Player;

namespace FlightRisk.Game
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private MoveController moveController;
        [SerializeField] private LookController lookController;
        [SerializeField] private GrabController grabController;
        [SerializeField] private InteractionController interactionController;
    }
}