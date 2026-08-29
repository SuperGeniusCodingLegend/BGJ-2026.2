using UnityEngine;

namespace FlightRisk.Game
{
    public class PlaneManager : MonoBehaviour
    {
        private const float STABILIZED_ANGLE_DEGREES = 0.0f;
        private const float MIN_ELEVATION_FEET = 0.0f;

        [SerializeField] private float maxElevationFeet = 35000.0f;
        [SerializeField] private float startElevationFeet = 25000.0f;
        [SerializeField] private float maxAngleDegrees = 45.0f;
        [SerializeField] private float startAngleDegrees = STABILIZED_ANGLE_DEGREES;
        [SerializeField] private float minAngleDegrees = -45.0f;
        [SerializeField] private float stallZoneAngleDegreesThreshold = 15.0f;
        [SerializeField] private float angleDangerZoneDegreesThreshold = -40.0f;
        [SerializeField] private float angleClimbRateDegreesPerSecond = 0.05f;
        [SerializeField] private float angleDiveRateDegreesPerSecond = 0.05f;
        [SerializeField] private float elevationDiveRateFeetPerSecondPerDegree = 2.5f;
        private bool isClimbing = false;
        private bool isStalling = false;
        private bool isInAngleDangerZone = false;
        private bool hasCrashed = false;
        private float currElevationFeet = 0.0f;
        private float currAngleDegrees = 0.0f;
        private float elevationDiveRateFeetPerSecond = 0.0f;
        private float angleChangeRateDegreesPerSecond = 0.0f;

        void Awake()
        {
            if (startElevationFeet > maxElevationFeet || startElevationFeet < MIN_ELEVATION_FEET)
            {
                Debug.LogError($"PlaneManager: startElevationFeet ({startElevationFeet}) must be between MIN_ELEVATION_FEET ({MIN_ELEVATION_FEET}) and maxElevationFeet ({maxElevationFeet})");
                startElevationFeet = Mathf.Clamp(startElevationFeet, MIN_ELEVATION_FEET, maxElevationFeet);
            }

            if (startAngleDegrees > maxAngleDegrees || startAngleDegrees < minAngleDegrees)
            {
                Debug.LogError($"PlaneManager: startAngleDegrees ({startAngleDegrees}) must be between minAngleDegrees ({minAngleDegrees}) and maxAngleDegrees ({maxAngleDegrees})");
                startAngleDegrees = Mathf.Clamp(startAngleDegrees, minAngleDegrees, maxAngleDegrees);
            }

            if (stallZoneAngleDegreesThreshold > maxAngleDegrees || stallZoneAngleDegreesThreshold < minAngleDegrees)
            {
                Debug.LogError($"PlaneManager: stallZoneAngleDegreesThreshold ({stallZoneAngleDegreesThreshold}) must be between minAngleDegrees ({minAngleDegrees}) and maxAngleDegrees ({maxAngleDegrees})");
                stallZoneAngleDegreesThreshold = Mathf.Clamp(stallZoneAngleDegreesThreshold, minAngleDegrees, maxAngleDegrees);
            }

            if (angleDangerZoneDegreesThreshold > maxAngleDegrees || angleDangerZoneDegreesThreshold < minAngleDegrees)
            {
                Debug.LogError($"PlaneManager: angleDangerZoneDegreesThreshold ({angleDangerZoneDegreesThreshold}) must be between minAngleDegrees ({minAngleDegrees}) and maxAngleDegrees ({maxAngleDegrees})");
                angleDangerZoneDegreesThreshold = Mathf.Clamp(angleDangerZoneDegreesThreshold, minAngleDegrees, maxAngleDegrees);
            }

            currElevationFeet = startElevationFeet;
            currAngleDegrees = startAngleDegrees;
            GameEvents.TrySubscribe((uint)FlightRisk.Game.GameEvents.Plane.PullingUp, OnPlanePullingUp);
        }

        void FixedUpdate()
        {
            if (hasCrashed)
            {
                return;
            }

            if (currElevationFeet <= MIN_ELEVATION_FEET)
            {
                GameEvents.TryInvoke((uint)GameEvents.Plane.Crash);
                hasCrashed = true;
                return;
            }

            if (currAngleDegrees < angleDangerZoneDegreesThreshold && !isInAngleDangerZone)
            {
                isInAngleDangerZone = true;
                GameEvents.TryInvoke((uint)GameEvents.Plane.AngleDangerZoneEntered);
            } else if (currAngleDegrees >= angleDangerZoneDegreesThreshold && isInAngleDangerZone)
            {
                isInAngleDangerZone = false;
                GameEvents.TryInvoke((uint)GameEvents.Plane.AngleDangerZoneExited);
            }

            angleChangeRateDegreesPerSecond = isClimbing ? angleClimbRateDegreesPerSecond : -angleDiveRateDegreesPerSecond;

            currAngleDegrees += angleChangeRateDegreesPerSecond * Time.fixedDeltaTime;
            currAngleDegrees = Mathf.Clamp(currAngleDegrees, minAngleDegrees, maxAngleDegrees);

            elevationDiveRateFeetPerSecond = -currAngleDegrees * elevationDiveRateFeetPerSecondPerDegree;

            if (currAngleDegrees > stallZoneAngleDegreesThreshold)
            {
                elevationDiveRateFeetPerSecond *= -1.0f;
                if (!isStalling)
                {
                    isStalling = true;
                    GameEvents.TryInvoke((uint)GameEvents.Plane.AngleStallingZoneEntered);
                }
            } else if (currAngleDegrees <= stallZoneAngleDegreesThreshold && isStalling)
            {
                isStalling = false;
                GameEvents.TryInvoke((uint)GameEvents.Plane.AngleStallingZoneExited);
            }

            currElevationFeet -= elevationDiveRateFeetPerSecond * Time.fixedDeltaTime;
            currElevationFeet = Mathf.Clamp(currElevationFeet, MIN_ELEVATION_FEET, maxElevationFeet);

            isClimbing = false;
        }

        private void OnPlanePullingUp(object eventPackage)
        {
            if (!hasCrashed)
            {
                isClimbing = true;   
            }
        }
    }
}
