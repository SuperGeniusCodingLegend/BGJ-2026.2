using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FlightRisk.Game.Tasks
{
    public class Task : MonoBehaviour
    {
        [SerializeField] protected List<TaskStep> steps;

        private int currentStepIndex;

        public virtual void TaskTick()
        {

        }

        private void AdvanceToNextStep()
        {
            currentStepIndex++;
        }
    }
}
