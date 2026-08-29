using UnityEngine;

namespace FlightRisk.Game.Tasks
{
    public class InteractStep : TaskStep
    {
        protected bool hasInteractedWith;

        public void TriggerInteract()
        {
            hasInteractedWith = true;
        }

        protected override bool CheckStepComplete()
        {
            return hasInteractedWith;
        }
    }
}
