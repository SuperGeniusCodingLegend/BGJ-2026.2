using UnityEngine;

namespace FlightRisk.Game.Player
{
    public class GrabController : MonoBehaviour
    {
        [SerializeField] private Transform grabRaycastOrigin;
        [SerializeField] private LayerMask grabbablesLayer;
        [SerializeField] private float interactRaycastLimit = 6;
        [SerializeField] private float delayBetweenInteractions = 0.25f;

        private IGrabbable currentGrabbedObject;

        private bool CheckForGrabbables(out IGrabbable grabbable)
        {
            grabbable = null;

            if (!InteractablesInRange(out var hit)) return false;
            if (!hit.collider.TryGetComponent(out grabbable)) return false;

            return true;
        }

        private bool InteractablesInRange(out RaycastHit hit)
        {
            return Physics.Raycast(
                grabRaycastOrigin.position,
                grabRaycastOrigin.forward,
                out hit,
                interactRaycastLimit,
                grabbablesLayer,
                QueryTriggerInteraction.Collide);
        }
    }
}