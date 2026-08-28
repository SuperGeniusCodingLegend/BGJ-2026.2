using UnityEngine;

namespace FlightRisk.Game.Player
{
    public class MoveController : Controller
    {
        [SerializeField] private Rigidbody body;
        [SerializeField] private Transform lookTarget;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float friction = 0.8f;

        private void FixedUpdate()
        {
            if (input && input.Move.IsActuated()) Move();
            else Stop();
        }

        private void Move()
        {
            var vel = lookTarget.InverseTransformVector(body.linearVelocity);
            vel.x = moveSpeed * input.Move.CurrentAxis.x;
            vel.z = moveSpeed * input.Move.CurrentAxis.y;
            body.linearVelocity = lookTarget.TransformVector(vel);
        }

        private void Stop()
        {
            var vel = body.linearVelocity;
            vel.x *= friction;
            vel.z *= friction;
            body.linearVelocity = vel;
        }
    }
}