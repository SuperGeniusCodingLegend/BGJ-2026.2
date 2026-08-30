using UnityEngine;
using FlightRisk.Game.Player;

namespace FlightRisk.Game
{
    public enum PlayerState { Idle, Grabbing, HoldingItem, Talking, Cockpit, }

    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private MoveController moveController;
        [SerializeField] private LookController lookController;
        [SerializeField] private GrabController grabController;
        [SerializeField] private InteractionController interactionController;
        [SerializeField] private DialogController dialogController;

        private void Awake()
        {
            GameEvents.TrySubscribe((uint)GameEvents.Dialog.Start, OnDialogStart);
            GameEvents.TrySubscribe((uint)GameEvents.Dialog.End, OnDialogEnd);
        }

        private void OnDialogStart(object payload)
        {
            moveController.enabled = false;
            lookController.enabled = false;
            grabController.enabled = false;
            interactionController.enabled = false;
        }

        private void OnDialogEnd(object payload)
        {
            moveController.enabled = true;
            lookController.enabled = true;
            grabController.enabled = true;
            interactionController.enabled = true;
        }
    }
}