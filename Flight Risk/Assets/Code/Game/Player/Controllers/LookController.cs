using UnityEngine;
using DG.Tweening;

namespace FlightRisk.Game.Player
{
    public class LookController : Controller
    {
        [SerializeField] private Transform verticalRotator;
        [SerializeField] private Transform horizontalRotator;
        [SerializeField] private Vector2 lookBaseSpeed;
        [SerializeField, Range(0, 90)] private float verticalLookLimit = 80;
        [SerializeField, Range(0, 180)] private float horizontalLookLimit = 0;

        private Vector3 lookTarget;

        private void Start()
        {
            lookTarget = verticalRotator.localEulerAngles;
        }

        private void Update()
        {
            if (!input) return;
            if (!input.Look.IsActuated()) return;

            Look();
        }

        private void Look()
        {
            lookTarget.y += lookBaseSpeed.x * input.Look.CurrentAxis.x;
            lookTarget.x += lookBaseSpeed.y * -input.Look.CurrentAxis.y;

            if (horizontalLookLimit != 0)
                lookTarget.y = Mathf.Clamp(lookTarget.y, -horizontalLookLimit, horizontalLookLimit);

            if (verticalLookLimit != 0) 
                lookTarget.x = Mathf.Clamp(lookTarget.x, -verticalLookLimit, verticalLookLimit);

            verticalRotator.localRotation = Quaternion.Euler(Vector3.right * lookTarget.x);
            horizontalRotator.localRotation = Quaternion.Euler(Vector3.up * lookTarget.y);
        }
    }
}
