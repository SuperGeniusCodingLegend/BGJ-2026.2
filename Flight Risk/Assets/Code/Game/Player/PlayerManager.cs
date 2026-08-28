using UnityEngine;
using FlightRisk.Game.Player;

namespace FlightRisk.Game
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private InputManager input;
        [SerializeField] private MoveController moveController;
        [SerializeField] private LookController lookController;
        [SerializeField] private InteractionController interactionController;

        private void Awake()
        {
            moveController.Setup(input);
            lookController.Setup(input);
            interactionController.Setup(input);
        }
    }
}