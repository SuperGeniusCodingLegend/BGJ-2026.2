using UnityEngine;

namespace FlightRisk.Game
{
    public interface IGrabbable
    {
        public Rigidbody GetBody();

        public void Grab();
        public void Hold();
        public void Release();
    }
}