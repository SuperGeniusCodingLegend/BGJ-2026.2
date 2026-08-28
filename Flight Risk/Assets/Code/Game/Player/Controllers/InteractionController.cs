using UnityEngine;
using FlightRisk.Game.Interactions;

namespace FlightRisk.Game.Player
{
    public class InteractionController : Controller
    {
        [SerializeField] private Transform interactRaycastOrigin;
        [SerializeField] private LayerMask interactableLayer;
        [SerializeField] private float interactRaycastLimit = 6;
        [SerializeField] private float delayBetweenInteractions = 0.25f;

        private Interactable currentInteractableInRange;
        private float interactTimer;

        private void Update()
        {
            HandleInteractableRaycast();

            if (CanInteract() && currentInteractableInRange && input && input.Secondary.WasActuatedThisFrame())
            {
                InteractWithCurrentInteractable();
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(interactRaycastOrigin.transform.position, interactRaycastOrigin.transform.position + (interactRaycastOrigin.transform.forward * interactRaycastLimit));
        }

        private bool CanInteract()
        {
            if (interactTimer > 0)
            {
                interactTimer -= Time.deltaTime;
                return false;
            }

            return true;
        }

        private void HandleInteractableRaycast()
        {
            if (CheckForCompatibleInteractables(out var interactable))
            {
                if (currentInteractableInRange != interactable)
                {
                    if (currentInteractableInRange) ExitCurrentInteractable();
                    EnterInteractable(interactable);
                }
            }
            else if (currentInteractableInRange)
            {
                ExitCurrentInteractable();
            }
        }

        private bool CheckForCompatibleInteractables(out Interactable compatibleInteractable)
        {
            compatibleInteractable = null;

            if (!InteractablesInRange(out var hit)) return false;

            if (currentInteractableInRange && hit.collider.gameObject == currentInteractableInRange.gameObject)
            {
                compatibleInteractable = currentInteractableInRange;
                return true;
            }

            if (!hit.collider.TryGetComponent(out compatibleInteractable)) return false;

            return true;
        }

        private bool InteractablesInRange(out RaycastHit hit)
        {
            return Physics.Raycast(
                interactRaycastOrigin.position,
                interactRaycastOrigin.forward,
                out hit,
                interactRaycastLimit,
                interactableLayer,
                QueryTriggerInteraction.Collide);
        }

        private void EnterInteractable(Interactable interactable)
        {
            currentInteractableInRange = interactable;
            currentInteractableInRange.EnterInteractRaycast();
            GameEvents.TryInvoke((uint)GameEvents.Interactions.Enter, interactable.ActionPrompt);
        }

        private void InteractWithCurrentInteractable()
        {
            currentInteractableInRange.Interact();
            interactTimer = delayBetweenInteractions;
        }

        private void ExitCurrentInteractable()
        {
            currentInteractableInRange.ExitInteractRaycast();
            currentInteractableInRange = null;
            GameEvents.TryInvoke((uint)GameEvents.Interactions.Exit);
        }
    }
}
