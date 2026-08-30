using UnityEngine;
using UnityEngine.EventSystems;
using FlightRisk.Game.Dialogs;

namespace FlightRisk.Game.Player
{
    public class DialogController : Controller , IRequireService<DialogManager>
    {
        private const float AXIS_THRESHOLD = 0.5f;

        [SerializeField] private float confirmCooldown = 0.2f;

        private DialogManager dialogManager;

        private float confirmTimer;
        private bool dialogActive;
        private bool horizontalInputConsumed;

        private void Start()
        {
            GameEvents.TrySubscribe((uint)GameEvents.Dialog.Start, OnDialogStart);
            GameEvents.TrySubscribe((uint)GameEvents.Dialog.End, OnDialogEnd);

            this.WaitForService<DialogManager>(diagMan => dialogManager = diagMan);
        }

        private void Update()
        {
            if (!dialogActive || !input) return;

            if (confirmTimer > 0) confirmTimer -= Time.deltaTime;

            float horizontalAxis = input.Move.CurrentAxis.x;

            if (Mathf.Abs(horizontalAxis) < AXIS_THRESHOLD)
            {
                horizontalInputConsumed = false;
            }
            else if (!horizontalInputConsumed)
            {
                dialogManager.CycleChoice(horizontalAxis > 0 ? -1: 1);
                horizontalInputConsumed = true;
            }

            if (confirmTimer <= 0 &&
                !EventSystem.current.IsPointerOverGameObject() &&
                    (input.Primary.WasActuatedThisFrame() ||
                    input.Secondary.WasActuatedThisFrame()))
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

        private void OnDialogEnd(object payload)
        {
            dialogActive = false;
        }
    }
}