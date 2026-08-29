using FlightRisk.Game.Dialogs;
using Unity.VisualScripting;
using UnityEngine;

namespace FlightRisk.Game.Player
{
    public class DialogController : Controller
    {
        private const float AXIS_THRESHOLD = 0.5f;
        [SerializeField] private DialogManager dialogManager;
        [SerializeField] private float confirmCooldown = 0.2f;
        private float confirmTimer;
        private bool dialogActive;
        private bool verticalInputConsumed;

        private void Awake()
        {
            base.Awake();
            GameEvents.TrySubscribe((uint)GameEvents.Dialog.Start, OnDialogStart);
            GameEvents.TrySubscribe((uint)GameEvents.Dialog.End, OnDialogEnd);
        }

        private void Update()
        {
            if (!dialogActive || !input)
            {
                return;
            }

            if (confirmTimer > 0)
            {
                confirmTimer -= Time.deltaTime;
            }

            float verticalAxis = input.Move.CurrentAxis.y;

            if (Mathf.Abs(verticalAxis) < AXIS_THRESHOLD)
            {
                verticalInputConsumed = false;
            }
            else if (!verticalInputConsumed)
            {
                dialogManager.CycleChoice(verticalAxis > 0 ? -1: 1);
                verticalInputConsumed = true;
            }

            if (confirmTimer <= 0 && (input.Primary.WasActuatedThisFrame() || input.Secondary.WasActuatedThisFrame()))
            {
                dialogManager.Confirm();
                confirmTimer = confirmCooldown;
            }
        }

        private void OnDialogStart(object payload)
        {
            dialogActive = true;
            confirmTimer = confirmCooldown;
        }
        
        private void OnDialogEnd(object payload) => dialogActive = false;
    }
}