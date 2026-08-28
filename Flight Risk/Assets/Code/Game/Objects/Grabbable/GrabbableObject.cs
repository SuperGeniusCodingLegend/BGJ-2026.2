using UnityEngine;

namespace FlightRisk.Game.Objects
{
    public class GrabbableObject : MonoBehaviour, IGrabbable
    {
        [SerializeField] private Rigidbody body;
        [SerializeField] private float damperWhenGrabbed = 5;

        private float originalLinearDamping;
        private float originalAngularDamping;

        private void Awake()
        {
            originalLinearDamping = body.linearDamping;
            originalAngularDamping = body.angularDamping;
        }

        public Rigidbody GetBody() => body;

        public void Grab()
        {
            body.linearDamping = damperWhenGrabbed;
            body.angularDamping = damperWhenGrabbed;
        }

        public void Hold()
        {
            
        }

        public void Release()
        {
            body.linearDamping = originalLinearDamping;
            body.angularDamping = originalAngularDamping;
        }
    }
}
