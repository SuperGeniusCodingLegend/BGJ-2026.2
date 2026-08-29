using FlightRisk.Game.Dialogs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FlightRisk.Game.Player
{
    public class DialogController : Controller
    {
        private const float AXIS_THRESHOLD = 0.5f;
        [SerializeField] private DialogManager dialogManager;
        [SerializeField] private float confirmCooldown = 0.2f;
        private float confirmTimer;
        private bool dialogActive;
        private bool horizontalInputConsumed;

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

            if (confirmTimer <= 0
                && !EventSystem.current.IsPointerOverGameObject()
                && (input.Primary.WasActuatedThisFrame() || input.Secondary.WasActuatedThisFrame()))
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