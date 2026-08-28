using UnityEngine;

namespace FlightRisk.Game
{
    public interface IGrabbable
    {
        public void Grab(Vector3 grabPosition);
        public void Hold(Vector3 holdPosition);
        public void Release(Vector3 releasePosition, Vector3 releaseForceVector);
    }
}