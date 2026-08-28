using UnityEngine;

namespace FlightRisk.Game.Player
{
    public class GrabController : Controller
    {
        [SerializeField] private Transform grabRaycastOrigin;
        [SerializeField] private Rigidbody grabberBody;
        [SerializeField] private LayerMask grabbablesLayer;
        [SerializeField] private float grabRaycastLimit = 4;
        [SerializeField] private float grabSpringForce = 400;
        [SerializeField] private float grabSpringDamper = 30;

        private IGrabbable currentGrabbedObject;
        private SpringJoint currentGrabJoint;
        private Vector3 grabPoint;
        private float grabForwardOffset;
        private bool isGrabbing;

        private void Update()
        {
            if (!isGrabbing)
            {
                if (CheckForGrabbables(out var grabbable) && input.Primary.WasActuatedThisFrame())
                {
                    GrabObject(grabbable);
                }

                return;
            }

            if (input.Primary.IsActuated()) 
                HoldCurrentObject();
            else 
                ReleaseCurrentObject();
        }

        private void OnDrawGizmosSelected()
        {
            if (!isGrabbing) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(grabRaycastOrigin.position + grabRaycastOrigin.forward * grabForwardOffset, 0.1f);
        }

        private void GrabObject(IGrabbable grabbable)
        {
            currentGrabbedObject = grabbable;

            grabberBody.position = grabPoint;
            currentGrabJoint = CreateHoldJoint(currentGrabbedObject.GetBody(), grabberBody, grabPoint);
            currentGrabbedObject.Grab();

            isGrabbing = true;
        }

        private void HoldCurrentObject()
        {
            currentGrabbedObject.Hold();
        }

        private void ReleaseCurrentObject()
        {
            Destroy(currentGrabJoint);
            currentGrabbedObject.Release();
            currentGrabbedObject = null;
            isGrabbing = false;
        }

        private bool CheckForGrabbables(out IGrabbable grabbable)
        {
            grabbable = null;

            if (!TryRaycastForGrabbables(out var hit)) return false;
            if (!hit.collider.TryGetComponent(out grabbable)) return false;
            if (!isGrabbing) grabPoint = hit.point;

            return true;
        }

        private bool TryRaycastForGrabbables(out RaycastHit hit)
        {
            return Physics.Raycast(
                grabRaycastOrigin.position,
                grabRaycastOrigin.forward,
                out hit,
                grabRaycastLimit,
                grabbablesLayer,
                QueryTriggerInteraction.Collide);
        }

        private SpringJoint CreateHoldJoint(Rigidbody grabbedBody, Rigidbody anchorBody, Vector3 worldGrabPoint)
        {
            var joint = grabbedBody.gameObject.AddComponent<SpringJoint>();

            joint.connectedBody = anchorBody;
            joint.spring = grabSpringForce;
            joint.damper = grabSpringDamper;
            joint.autoConfigureConnectedAnchor = false;
            joint.anchor = grabbedBody.transform.InverseTransformPoint(worldGrabPoint);
            joint.connectedAnchor = Vector3.zero;

            return joint;
        }
    }
}